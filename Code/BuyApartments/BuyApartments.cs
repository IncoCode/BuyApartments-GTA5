#region Using

using System;
using System.Windows.Forms;
using BuyApartments.Controller;
using BuyApartments.Model;
using GTA;
using iFruitAddon;
using Ini;
using NativeUI;

#endregion

namespace BuyApartments
{
    public class BuyApartments : Script
    {
        private readonly IniFile _settings;
        private readonly CustomiFruit _fruit;
        private readonly HouseController _houseController;
        private readonly PurchasedHousesController _purchasedHousesController;
        private readonly MenuPool _menuPool;
        private UIMenu _menu;
        private Blip _blip;

        public BuyApartments()
        {
            this._houseController = new HouseController();
            this._purchasedHousesController = new PurchasedHousesController( this._houseController );
            this._menuPool = new MenuPool();

            this._fruit = new CustomiFruit();
            var contact = new iFruitContact( "Apartment Dealer", 10 );
            contact.Selected += ( sender, args ) => this.CallDealer();
            this._fruit.Contacts.Add( contact );

            this.Interval = 1;
            this.Tick += this.BuyApartments_Tick;
            this.KeyDown += this.BuyApartments_KeyDown;
        }

        private void BuyApartments_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.I )
            {
                this.CallDealer();
            }
        }

        private void BuyApartments_Tick( object sender, EventArgs e )
        {
            this._fruit.Update();
            this._menuPool.ProcessMenus();
        }

        private void CreateApartmentsMenu()
        {
            var menu = new UIMenu( "Buy Apartments", "" );
            foreach ( House house in this._purchasedHousesController.FreeHouses )
            {
                var button = new UIMenuItem( house.Name );
                button.Activated += ( sender, item ) =>
                {
                    this._blip = World.CreateBlip( house.Location );
                    this._blip.ShowRoute = true;
                    this._menuPool.CloseAllMenus();
                };
                menu.AddItem( button );
            }
            menu.Visible = true;
            menu.RefreshIndex();
            this._menuPool.Add( menu );
        }

        private void CallDealer()
        {
            this.CreateApartmentsMenu();
        }
    }
}