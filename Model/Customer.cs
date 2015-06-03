using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
    //made by 廖开翔
    public class Customer :ICustomer
    {
        public string Name { get; set; }//顾客的姓名
        public string Gender { get; set; }//顾客的性别
        public int Age { get;set; }//顾客的年龄
        public string Phone { get; set; }//顾客的电话
        public int ID { get; set; }//顾客的身份证号码
        public string Company { get; set; }//顾客所在单位
        public string City { get; set; }//顾客居住的城市
        public CustomerStatus CStatus { get; set; }//顾客当前的状态

        private List<IBooking> bookings;//订单
        public Customer(string name, string gender,int age, string phone,
            int id, string company, string city, CustomerStatus status)
        {
            Name = name;
            Gender = gender;
            Age = age;
            Phone = phone;
            ID = id;
            Company = company;
            City = city;
            CStatus = status;
            bookings = new List<IBooking>();
        }
        public Customer(int id)
        {
            ID = id;
            bookings = new List<IBooking>();
        }

    }
}
