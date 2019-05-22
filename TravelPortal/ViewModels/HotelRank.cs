using System;
using System.Collections.Generic;
using System.Linq;
namespace TravelPortal.ViewModels
{
    public class HotelRank
    {
        public string Name { get; }
        public string City { get; }
        public int Type { get; }
        public long VoucherClount { get; }
        public double Popularity { get; }

        public HotelRank(string name, string city, int type, long voucherClount, double popularity)
        {
            Name = name;
            City = city;
            Type = type;
            VoucherClount = voucherClount;
            Popularity = popularity;
        }
    }
}
