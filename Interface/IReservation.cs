using System;

namespace Interface
{
    public interface IReservation
    {
        string ID                     { get; set; }                 //订单号
        double Payment                { get; set; }                 //应付的总费用
        double DownPayment            { get; set; }                 //定金
        ReservationStatus RStatus     { get; set; }                 //结算状态
    }
}
