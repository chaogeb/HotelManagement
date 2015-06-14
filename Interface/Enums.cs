// Made by chaogeb
namespace Interface
{
    public enum CustomerGender
    {
        Male,
        Female
    }

    // Booking status
    public enum BookStatus
    {
        Confirmed,  // 确认
        Timeout,    // 超时
        Canceled    // 取消
    }

    // Reservation status
    public enum ReservationStatus
    {
        Booked,     // 确认
        Default,    // 违约
        Canceled,   // 取消
        Paid        // 已付
    }

    // Room status
    public enum RoomStatus
    {
        Idle,       // 空闲
        Occupied,   // 入住
        NA          // Not Available
    }

    // Room type
    public enum RoomType
    {
        Junior,
        Double,
        Triple,
        OneBed,
        TwoBed,
        EconTwin
    }
}
