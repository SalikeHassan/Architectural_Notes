public interface IParkingRepository
{
    Task AddParkingWithSlotsAsync(Parking parking);
    Task<Parking> GetParkingAsync(int parkingId);
}

public interface IInventoryRepository
{
    Task AddInventoryAsync(List<ParkingSlotInventory> inventories);
    Task<ParkingSlotInventory> GetInventoryAsync(int parkingId, VehicleType vehicleType);
    Task UpdateInventoryAsync(ParkingSlotInventory inventory);
}

public interface IReservationRepository
{
    Task AddReservationAsync(ParkingReservation reservation);
    Task<ParkingSlot> GetAvailableSlotAsync(int parkingId, VehicleType vehicleType);
    Task UpdateParkingSlotAsync(ParkingSlot slot);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public IParkingRepository ParkingRepository { get; }
    public IInventoryRepository InventoryRepository { get; }
    public IReservationRepository ReservationRepository { get; }

    public UnitOfWork(DbContext context,
        IParkingRepository parkingRepository,
        IInventoryRepository inventoryRepository,
        IReservationRepository reservationRepository)
    {
        _context = context;
        ParkingRepository = parkingRepository;
        InventoryRepository = inventoryRepository;
        ReservationRepository = reservationRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

public class CreateParkingCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateParkingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(CreateParkingCommand command)
    {
        using var transaction = await _unitOfWork.SaveChangesAsync();

        try
        {
            // Create Parking with Slots
            var parking = new Parking
            {
                Name = command.Name,
                TwoWheelerCapacity = command.TwoWheelerCapacity,
                FourWheelerCapacity = command.FourWheelerCapacity,
                HeavyWheelerCapacity = command.HeavyWheelerCapacity,
                ParkingSlots = command.ParkingSlots.Select(slot => new ParkingSlot
                {
                    SlotNumber = slot.SlotNumber,
                    VehicleType = slot.VehicleType,
                    IsOccupied = false
                }).ToList()
            };

            await _unitOfWork.ParkingRepository.AddParkingWithSlotsAsync(parking);

            // Create Inventory
            var inventories = command.ParkingSlots
                .GroupBy(slot => slot.VehicleType)
                .Select(group => new ParkingSlotInventory
                {
                    ParkingId = parking.ParkingId,
                    VehicleType = group.Key,
                    TotalSlots = group.Count(),
                    OccupiedSlots = 0
                }).ToList();

            await _unitOfWork.InventoryRepository.AddInventoryAsync(inventories);

            await _unitOfWork.SaveChangesAsync();

            return new Response { Success = true, Message = "Parking and slots created successfully" };
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = ex.Message };
        }
    }
}

public class ReserveParkingSlotCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ReserveParkingSlotCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(ReserveParkingSlotCommand command)
    {
        using var transaction = await _unitOfWork.SaveChangesAsync();

        try
        {
            // Check Inventory
            var inventory = await _unitOfWork.InventoryRepository.GetInventoryAsync(command.ParkingId, command.VehicleType);

            if (inventory == null || (inventory.TotalSlots - inventory.OccupiedSlots) <= 0)
            {
                return new Response { Success = false, Message = "No available slots for the specified vehicle type" };
            }

            // Get Available Slot
            var slot = await _unitOfWork.ReservationRepository.GetAvailableSlotAsync(command.ParkingId, command.VehicleType);

            if (slot == null)
            {
                return new Response { Success = false, Message = "No available slot found" };
            }

            // Create Reservation
            var reservation = new ParkingReservation
            {
                ParkingId = command.ParkingId,
                SlotId = slot.SlotId,
                VehicleNumber = command.VehicleNumber,
                InTime = command.InTime,
                OutTime = null
            };

            await _unitOfWork.ReservationRepository.AddReservationAsync(reservation);

            // Update Slot Status
            slot.IsOccupied = true;
            await _unitOfWork.ReservationRepository.UpdateParkingSlotAsync(slot);

            // Update Inventory
            inventory.OccupiedSlots += 1;
            await _unitOfWork.InventoryRepository.UpdateInventoryAsync(inventory);

            await _unitOfWork.SaveChangesAsync();

            return new Response
            {
                Success = true,
                Message = "Slot reserved successfully",
                Data = new
                {
                    ReservationId = reservation.ReservationId,
                    SlotId = slot.SlotId,
                    ParkingId = command.ParkingId,
                    VehicleType = command.VehicleType,
                    VehicleNumber = command.VehicleNumber,
                    InTime = command.InTime
                }
            };
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = ex.Message };
        }
    }
}

