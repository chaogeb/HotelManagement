using System;
using System.Collections.Generic;

namespace Interface
{
    // Made by chaogeb
    public interface ICustomer
    {
        string Name { get; set; }
        string Gender { get; set; }
        int Age { get; set; }
        string ID { get; set; }
        string Company { get; set; }
        string City { get; set; }
        CustomerStatus CStatus { get; set; }
    }
}
