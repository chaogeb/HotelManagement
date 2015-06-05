using System;

namespace Interface
{
    // Made by chaogeb
    public interface IBooking
    {
        string ID { get; private set; }     //订单识别码
        DateTime StartDate { get; set; }             //起始日期
        DateTime EndDate { get; set; }            //终止日期   
        string ReserveTime { get; set; }            //到店具体时间
        string ContractID { get; set; }            //联系人ID
        RoomType Roomtype { get; set; }            //房间类型
        string RoomID { get; set; }            //房间ID
        string ReservationID { get; set; }            //结账单号
        BookStatus BStatus { get; set; }            //订单状态
    }
}
