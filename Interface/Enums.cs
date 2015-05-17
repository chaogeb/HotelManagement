using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Made by chaogeb
namespace Interface
{
    public enum QualityStars
    {
        OneStar,
        TwoStars,
        ThreeStars,
        FourStars,
        FiveStars,
    }

    public enum CustomerStatus
    {
        CheckedIn,
        CheckedOut,
        Waiting
    }

    public enum BookingType
    {
        AllIncluded,
        Breakfast,
        TwoMeals
    }

    public enum BookStatus
    {
        Canceled,
        Completed,
        Confirmed,
        Paid
    }

    public enum RoomStatus
    {
        Indisposed,
        Occupied,
        Ready
    }

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
