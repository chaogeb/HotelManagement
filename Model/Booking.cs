using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
    //made by 廖开翔
    public class Booking:IBooking
    {
        public string ID                   { get; private set; }     //订单识别码
        public DateTime StartDate          { get; set; }             //起始日期
        public DateTime EndDate            { get; set; }            //终止日期   
        public string ReserveTime          { get; set; }            //到店具体时间
        public string ContractID           { get; set; }            //联系人ID
        public RoomType Roomtype           { get; set; }            //房间类型
        public string RoomID               { get; set; }            //房间ID
        public string ReservationID        { get; set; }            //结账单号
        public BookStatus BStatus          { get; set; }            //订单状态
        public Booking(string id, DateTime start, DateTime end, string reserveTime,
                       string contractID, RoomType roomType, string roomId,string reservationID)
        {
            ID = id;
            StartDate = start;
            EndDate = end;
            ReserveTime = reserveTime;
            ContractID = contractID;
            Roomtype = roomType;
            RoomID = roomId;
            ReservationID = reservationID;
            BStatus = BookStatus.Confirmed;
        }

        public Booking()
        {
            ID = IClock.GetBookingCount;
        }
    }
}
