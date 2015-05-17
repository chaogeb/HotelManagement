using System;
using System.Collections.Generic;

namespace Interface
{
    // Made by chaogeb
    public interface ICustomer
    {
        int ID { get; }

        string Name { get; set; }

        string StreetName { get; set; }

        string City { get; set; }

        string Country { get; set; }

        string Phone { get; set; }

        string Fax { get; set; }

        string Email { get; set; }

        CustomerStatus CStatus { get; set; }

    }
}
