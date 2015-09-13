#region Using

using System.Collections.Generic;

#endregion

namespace BuyApartments.Model.JSON
{
    internal class InteriorJSON
    {
        public CoordinatesJSON Coordinates { get; set; }
        public string Name { get; set; }
        public float Heading { get; set; }
    }

    internal class InteriorsListJSON
    {
        public List<InteriorJSON> Interiors { get; set; }
    }
}