using System.Collections.Generic;

namespace BuyApartments.Model
{
    internal class Interior
    {
        public Coordinates Coordinates { get; set; }
        public string Name { get; set; }
    }

    internal class InteriorsList
    {
        public List<Interior> Interiors { get; set; }
    }
}
