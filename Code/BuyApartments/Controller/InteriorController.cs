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
    internal class InteriorController
    {
        private readonly List<Interior> _interiors;

        public InteriorController()
        {
            this._interiors = new List<Interior>();

            // parse default interiors
            this.FillList( Properties.Resource.Interiors );
            this.ParseCustomInteriors();
        }

        private void FillList( string json )
        {
            InteriorsListJSON parsedInteriors;
            try
            {
                parsedInteriors = JsonConvert.DeserializeObject<InteriorsListJSON>( json );
            }
            catch
            {
                return;
            }
            foreach (
                var interior in
                    parsedInteriors.Interiors.Select(
                        interiorJSON => new Interior( interiorJSON.Coordinates, interiorJSON.Name ) ) )
            {
                if ( this._interiors.Contains( interior ) )
                {
                    this._interiors.Remove( interior );
                }
                this._interiors.Add( interior );
            }
        }

        private void ParseCustomInteriors()
        {
            const string filename = @"scripts\BuyApartments\CustomInterior.json";
            if ( !File.Exists( filename ) )
            {
                return;
            }
            this.FillList( File.ReadAllText( filename ) );
        }
    }
}