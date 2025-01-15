sequenceDiagram
   participant User
   participant ParkingService
   participant InventoryService
   participant BookingService
   participant BillingService
   participant PaymentService
   participant Stripe

   %% Step1: Create Parking and Slots
      User ->> ParkingService: CreateParkingAndSlotsCommand
      ParkingService ->> ParkingService: Create Parking and Slots
      ParkingService ->> InventoryService: CreateNewInventoryForParkingCommand
      InventoryService ->> InventoryService: Create inventory
    
   %% Step2: Get Available Slots
      User  ->> InventoryService: GetAvailableSlotBasedOnParkingIdAndVehivleType
      InventoryService ->> User: Response
    
   %% Step3: Book Parking Slot
      User ->> BookingService: BookNewParking
      BookingService ->> BookingService: Create
      BookingService ->> InventoryService: UpdateInventoryCommand

   %% Step4: Make payment
      User ->> BillingService: GetPaymentDetailsQuery
      BillingService ->> User: Response
      User ->> BillingService: MakePaymentCommand
      BillingService ->> PaymentService: ProcessPaymentCommand
      PaymentService ->> Stripe: ProcessPayment
      Stripe -->> PaymentService: Response webhook
      BillingService ->> User: Success, Exit