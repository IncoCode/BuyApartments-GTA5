using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using iFruitAddon;
using Ini;

namespace BuyApartments
{
    public class BuyApartments : Script
    {
        private readonly IniFile _settings;
        private readonly CustomiFruit _fruit;

        public BuyApartments()
        {
            this.Tick += this.BuyApartments_Tick;

            this._fruit = new CustomiFruit();
            var contact = new iFruitContact( "Apartment Dealer", this._fruit.Contacts.Count );
            contact.Selected += ( sender, args ) => UI.ShowSubtitle( "1" );
            this._fruit.Contacts.Add( contact );
        }

        private void BuyApartments_Tick( object sender, EventArgs e )
        {
            this._fruit.Update();
        }
    }
}
