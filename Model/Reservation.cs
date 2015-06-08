using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
    public class Reservation:IReservation
    {
        public string ID { get; set; }                 //订单号
        public double Payment { get; set; }                 //应付的总费用
        public double DownPayment { get; set; }                 //定金
        public ReservationStatus RStatus { get; set; }                 //结算状态
        public Reservation(string id,double payment,double downpayment,ReservationStatus rstatus)
        {
            ID = id;
            Payment = payment;
            DownPayment = downpayment;
            RStatus = rstatus;
        }
        public Reservation()
        {
            ID = IClock.GetReservID;
        }
    }
}
