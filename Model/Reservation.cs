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
        public string Payment { get; set; }                 //应付的总费用
        public string DownPayment { get; set; }                 //定金
        public ReservationStatus RStatus { get; set; }                 //结算状态
        public Reservation(string id,string payment,string downpayment,ReservationStatus rstatus)
        {
            ID = id;
            Payment = payment;
            DownPayment = downpayment;
            RStatus = rstatus;
        }
    }
}
