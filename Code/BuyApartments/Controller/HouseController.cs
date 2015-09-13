#region Using

using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuyApartments.Model;
using BuyApartments.Model.JSON;
using Newtonsoft.Json;

#endregion

namespace BuyApartments.Controller
{
    internal class HouseController
    {
        private readonly List<House> _houses;
        private readonly InteriorController _interiorController;

        internal List<House> Houses => this._houses;

        public HouseController()
        {
            this._houses = new List<House>();
            this._interiorController = new InteriorController();

            // parse default houses
            this.FillList( Properties.Resource.Houses );
            this.ParseCustomHouses();
        }

        private void FillList( string json )
        {
            HousesListJSON parsedInteriors;
            try
            {
                parsedInteriors = JsonConvert.DeserializeObject<HousesListJSON>( json );
            }
            catch
            {
                return;
            }
            foreach ( HouseJSON houseJSON in parsedInteriors.Houses )
            {
                Interior interior = this._interiorController.Interiors.FirstOrDefault( i => i.Name == houseJSON.Interior );
                if ( interior == null )
                {
                    return;
                }
                var house = new House( houseJSON.Name, interior, houseJSON.Price, houseJSON.Location,
                    houseJSON.DailyRent );
                if ( this._houses.Contains( house ) )
                {
                    this._houses.Remove( house );
                }
                this._houses.Add( house );
            }
        }

        private void ParseCustomHouses( string filename = @"scripts\BuyApartments\CustomHouses.json" )
        {
            if ( !File.Exists( filename ) )
            {
                return;
            }
            this.FillList( File.ReadAllText( filename ) );
        }
    }
}