using System.Collections.Generic;
using BuyApartments.Model;
using BuyApartments.Model.JSON;
using GTA.Math;
using Newtonsoft.Json;

namespace BuyApartments.Controller
{
    internal class InteriorController
    {
        private readonly List<Interior> _interiors;

        public InteriorController()
        {
            this._interiors = new List<Interior>();

            InteriorsListJSON parsedInteriors = JsonConvert.DeserializeObject<InteriorsListJSON>( Properties.Resource.Interiors );
            foreach ( InteriorJSON interiorJSON in parsedInteriors.Interiors )
            {
                var interior = new Interior( interiorJSON.Coordinates, interiorJSON.Name );
            }
        }
    }
}
