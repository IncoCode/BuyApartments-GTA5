using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyApartments.Model
{
    public class Location
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class House
    {
        public string Name { get; set; }
        public string Interior { get; set; }
        public int Price { get; set; }
        public Location Location { get; set; }
    }

    public class HousesList
    {
        public List<House> Houses { get; set; }
    }
}
