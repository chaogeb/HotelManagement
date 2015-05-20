using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Model
{
    public class Hotel : IHotel
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public QualityStars Quality { get; set; }

        public List<Hotel> hotel { get; set; }

        /// <summary>
        /// Initializes a Hotel.
        /// </summary>
        /// <param name="id">Datebase generated id.</param>
        /// <param name="name">Name of the hotel.</param>
        /// <param name="street">Street name of the hotel.</param>
        /// <param name="city">City of the hotel.</param>
        /// <param name="email">Contact email for the hotel.</param>
        /// <param name="phone">Contact phone number for the hotel.</param>
        /// <param name="fax">Contact fax number for the hotel.</param>
        /// <param name="quality">The number of stars or the quality of the hotel.</param>
        public Hotel(int id, string name, string street, string city,
                     string email, string phone, string fax, QualityStars quality)
        {
            ID = id;
            Name = name;
            StreetName = street;
            City = city;
            Email = email;
            Phone = phone;
            Fax = fax;
            Quality = quality;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    
}
