using System;
using Interface;

namespace Model
{
    //made by 廖开翔
    public class Booking : IBooking
    {
        public string ID                   { get; set; }            //订单识别码
        public DateTime StartDate          { get; set; }            //起始日期
        public DateTime EndDate            { get; set; }            //终止日期   
        public string ReserveTime          { get; set; }            //到店具体时间
        public string ContractID           { get; set; }            //联系人ID
        public RoomType Roomtype           { get; set; }            //房间类型
        public double ThisPrice            { get; set; }            //订单当前房间价格
        public string RoomID               { get; set; }            //房间ID
        public string ReservationID        { get; set; }            //结账单号
        public BookStatus BStatus          { get; set; }            //订单状态
        public Booking(string id, DateTime start, DateTime end, string reserveTime,
                       string contractID, RoomType roomType, double price, string roomId,string reservationID, BookStatus bookingStatus)
        {
            ID = id;
            StartDate = start;
            EndDate = end;
            ReserveTime = reserveTime;
            ContractID = contractID;
            Roomtype = roomType;
            ThisPrice = price;
            RoomID = roomId;
            ReservationID = reservationID;
            BStatus = bookingStatus;
        }

        public Booking()
        {
            ID = IClock.GetBookingID;
        }
    }
}
