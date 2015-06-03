using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Made by chaogeb
namespace Interface
{
    //酒店星级
    public enum QualityStars
    {
        OneStar,
        TwoStars,
        ThreeStars,
        FourStars,
        FiveStars,
    }
    //顾客状态
    public enum CustomerStatus
    {
        CheckedIn,
        CheckedOut,
        Waiting
    }
    //订单类型
    public enum BookingType
    {
        AllIncluded,
        Breakfast,
        TwoMeals
    }
    //订单状态
    public enum BookStatus
    {
        Canceled,
        Completed,
        Confirmed,
        Paid
    }
    //房间状态
    public enum RoomStatus
    {
        Indisposed,
        Occupied,
        Ready
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
