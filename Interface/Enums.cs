using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Made by chaogeb
namespace Interface
{
    public enum CustomerGender
    {
        Male,
        Female
    }
    //订单状态
    public enum BookStatus
    {
        Confirmed,  // 确认
        Timeout,    // 超时
        Canceled    // 取消
    }
    // 结算状态
    public enum ReservationStatus
    {
        Booked,     // 确认
        Default,    // 违约
        Canceled,   // 取消
        Paid        // 已付
    }
    //房间状态
    public enum RoomStatus
    {
        Idle,       // 空闲
        Occupied,   // 入住
        NA          // Not Available
    }
    //房间类型
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
