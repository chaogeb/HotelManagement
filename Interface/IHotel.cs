using System;

namespace Interface
{
    // Made by chaogeb
    public interface IHotel
    {
        int ID { get; }

        string Name { get; set; }

        string StreetName { get; set; }

        string City { get; set; }

        string Email { get; set; }

        string Phone { get; set; }

        string Fax { get; set; }

        QualityStars Quality { get; set; }
    }
}
