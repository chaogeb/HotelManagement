using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
    //made by 廖开翔
    public class Customer : ICustomer
    {
        public string ID                { get; set; }   //唯一识别码
        public string Name              { get; set; }   //顾客的姓名
        public CustomerGender Gender    { get; set; }   //顾客的性别
        public int Age                  { get;set; }    //顾客的年龄
        public string Phone             { get; set; }   //顾客的电话
        public string Fax               { get; set; }
        public string IDcard            { get; set; }   //顾客的身份证号码
        public string RoomID            { get; set; }   //房间ID
        public string Company           { get; set; }   //顾客所在单位
        public string Address           { get; set; }   //地址
        public string City              { get; set; }   //顾客居住的城市

        private List<IBooking> bookings;//订单
        public Customer(string id, string name, CustomerGender gender, int age, string phone,
            string fax, string idcard, string roomid, string company, string address, string city)
        {
            ID = id;
            Name = name;
            Gender = gender;
            Age = age;
            Phone = phone;
            Fax = fax;
            IDcard = idcard;
            RoomID = roomid;
            Company = company;
            Address = address;
            City = city;
            bookings = new List<IBooking>();
        }
        public Customer(string id)
        {
            ID = id;
            bookings = new List<IBooking>();
        }

    }
}
