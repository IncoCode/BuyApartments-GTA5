#region Using

using System.Collections.Generic;
using System.Linq;
using BuyApartments.Model;

#endregion

namespace BuyApartments.Controller
{
    internal class PurchasedHousesController
    {
        private readonly List<House> _purchasedHouses;
        private readonly HouseController _houseController;

        #region Fields

        public House[] FreeHouses
        {
            get { return this._houseController.Houses.Where( h => !this._purchasedHouses.Contains( h ) ).ToArray(); }
        }

        public List<House> PurchasedHouses => this._purchasedHouses;

        #endregion

        public PurchasedHousesController( HouseController houseController )
        {
            this._houseController = houseController;
            this._purchasedHouses = new List<House>();
            this.LoadPurchasedHouses();
        }

        private void LoadPurchasedHouses()
        {
            // ToDo: load from file
        }

        public void BuyHouse( House house )
        {
            this._purchasedHouses.Add( house );
            // ToDo: save data
        }

        public bool IsPlayerBoughtHouse( House house )
        {
            return this._purchasedHouses.Contains( house );
        }
    }
}