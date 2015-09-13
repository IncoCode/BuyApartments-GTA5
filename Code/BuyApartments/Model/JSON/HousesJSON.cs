#region Using

using System.Collections.Generic;

#endregion

namespace BuyApartments.Model.JSON
{
    internal class HouseJSON
    {
        public string Name { get; set; }
        public string Interior { get; set; }
        public int Price { get; set; }
        public CoordinatesJSON Location { get; set; }
    }

    internal class HousesListJSON
    {
        public List<HouseJSON> Houses { get; set; }
    }
}