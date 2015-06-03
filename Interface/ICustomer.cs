using System;
using System.Collections.Generic;

namespace Interface
{
    // Made by chaogeb
    public interface ICustomer
    {
        string Name { get; set; }//顾客的姓名
        string Gender { get; set; }//顾客的性别
        int Age { get; set; }//顾客的年龄
        string Phone { get; set; }//顾客的电话
        int ID { get; set; }//顾客的身份证号码
        string Company { get; set; }//顾客所在单位
        string City { get; set; }//顾客居住的城市
        CustomerStatus CStatus { get; set; }//顾客当前的状态
    }
}
