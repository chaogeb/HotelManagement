using System;

namespace Interface
{
    public interface IReservation
    {
        string ID                     { get; set; }                 //订单号
        string Payment                { get; set; }                 //应付的总费用
        string DownPayment            { get; set; }                 //定金
        ReservationStatus RStatus     { get; set; }                 //结算状态
    }
}
