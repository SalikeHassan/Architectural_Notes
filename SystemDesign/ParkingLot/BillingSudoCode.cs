public class ReservationDetails
{
    public int ReservationId { get; set; }
    public int ParkingId { get; set; }
    public VehicleType VehicleType { get; set; }
    public DateTime InTime { get; set; }
    public DateTime OutTime { get; set; }
    public int DurationInMinutes => (int)(OutTime - InTime).TotalMinutes;
}

public class RateDetails
{
    public decimal BaseRate { get; set; }
    public decimal ExtendedRate { get; set; }
}


public interface IReservationRepository
{
    Task<ReservationDetails> GetReservationDetailsAsync(int reservationId);
}

public class ReservationRepository : IReservationRepository
{
    private readonly DbContext _context;

    public ReservationRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<ReservationDetails> GetReservationDetailsAsync(int reservationId)
    {
        return await _context.Set<ParkingReservation>()
            .Where(r => r.ReservationId == reservationId)
            .Select(r => new ReservationDetails
            {
                ReservationId = r.ReservationId,
                ParkingId = r.ParkingId,
                VehicleType = r.VehicleType,
                InTime = r.InTime,
                OutTime = r.OutTime
            })
            .FirstOrDefaultAsync();
    }
}

public interface IRateRepository
{
    Task<RateDetails> GetRateDetailsAsync(int parkingId, VehicleType vehicleType);
}

public class RateRepository : IRateRepository
{
    private readonly DbContext _context;

    public RateRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<RateDetails> GetRateDetailsAsync(int parkingId, VehicleType vehicleType)
    {
        return await _context.Set<Rate>()
            .Where(r => r.ParkingId == parkingId && r.VehicleType == vehicleType)
            .Select(r => new RateDetails
            {
                BaseRate = r.BaseRate,
                ExtendedRate = r.ExtendedRate
            })
            .FirstOrDefaultAsync();
    }
}

public class CalculateBillQueryHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CalculateBillQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(int reservationId)
    {
        // Step 1: Fetch Reservation Details
        var reservationDetails = await _unitOfWork.ReservationRepository.GetReservationDetailsAsync(reservationId);

        if (reservationDetails == null)
            return new Response { Success = false, Message = "Reservation not found." };

        // Step 2: Fetch Rate Details
        var rateDetails = await _unitOfWork.RateRepository.GetRateDetailsAsync(
            reservationDetails.ParkingId,
            reservationDetails.VehicleType
        );

        if (rateDetails == null)
            return new Response { Success = false, Message = "Rate details not found for the parking and vehicle type." };

        // Step 3: Calculate Duration in Minutes
        var durationInMinutes = reservationDetails.DurationInMinutes;

        // Step 4: Calculate Total Price
        decimal totalPrice;
        if (durationInMinutes <= 120) // 2 hours = 120 minutes
        {
            totalPrice = rateDetails.BaseRate;
        }
        else
        {
            var additionalMinutes = durationInMinutes - 120;
            var additionalHours = (decimal)additionalMinutes / 60; // Convert remaining minutes to fractional hours
            totalPrice = rateDetails.BaseRate + (additionalHours * rateDetails.ExtendedRate);
        }

        // Step 5: Prepare Response
        return new Response
        {
            Success = true,
            Message = "Calculation successful.",
            Data = new
            {
                ReservationId = reservationDetails.ReservationId,
                ParkingId = reservationDetails.ParkingId,
                VehicleType = reservationDetails.VehicleType,
                DurationInMinutes = durationInMinutes,
                TotalPrice = totalPrice
            }
        };
    }
}