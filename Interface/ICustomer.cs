using System;
using System.Collections.Generic;

namespace Interface
{
    // Made by chaogeb
    public interface ICustomer
    {
        string ID               { get; set; }   //唯一识别码
        string Name             { get; set; }   //顾客的姓名
        CustomerGender Gender   { get; set; }   //顾客的性别
        int Age                 { get; set; }   //顾客的年龄
        string Phone            { get; set; }   //顾客的电话
        string Fax              { get; set; }   //传真
        string IDcard           { get; set; }   //顾客的身份证号码
        string RoomID           { get; set; }   //房间ID
        string Company          { get; set; }   //顾客所在单位
        string Address          { get; set; }   //地址
    }
}
