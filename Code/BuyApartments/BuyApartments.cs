﻿#region Using

using System;
using System.Linq;
using System.Threading;
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
        private bool _aroundSomeHouse = false;

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
            var player = Game.Player;
            House aroundHouse;
            if ( !this._aroundSomeHouse && this.AroundSomeHouse( out aroundHouse ) )
            {
                this._aroundSomeHouse = true;
                player.CanControlCharacter = false;
                this._blip?.Remove();
                this._blip = null;
                this.CreateHouseMenu( aroundHouse );
            }
        }

        private bool AroundSomeHouse( out House house )
        {
            house = this._houseController.Houses.FirstOrDefault( h => h.IsInRange( Game.Player.Character.Position ) );
            return house != null;
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

        private void CreateHouseMenu( House house )
        {
            var menu = new UIMenu( "House Menu", "" );
            House[] boughtHouses =
                this._purchasedHousesController.PurchasedHouses.Where( h => h.Location == house.Location ).ToArray();
            House[] freeHouses =
                this._purchasedHousesController.FreeHouses.Where( h => h.Location == house.Location ).ToArray();
            foreach ( House boughtHouse in boughtHouses )
            {
                var bButton = new UIMenuItem( "Enter " + boughtHouse.Name );
                bButton.Activated += ( sender, item ) =>
                {
                    this._menuPool.CloseAllMenus();
                    Player player = Game.Player;
                    Game.FadeScreenIn( 500 );
                    Wait( 500 );
                    player.Character.Position = boughtHouse.Interior.StartPoint;
                    player.Character.Heading = boughtHouse.Interior.Heading;
                    Game.FadeScreenOut( 500 );
                    player.CanControlCharacter = true;
                };
                menu.AddItem( bButton );
            }
            foreach ( House freeHouse in freeHouses )
            {
                var bButton = new UIMenuItem( "Buy " + freeHouse.Name );
                bButton.Activated += ( sender, item ) =>
                {
                    Player player = Game.Player;
                    if ( player.Money < freeHouse.Price )
                    {
                        UI.Notify( "~r~Error:~r~ Not enough money!" );
                        return;
                    }
                    this._purchasedHousesController.BuyHouse( freeHouse );
                    this._menuPool.CloseAllMenus();
                    this.CreateHouseMenu( freeHouse );
                };
                menu.AddItem( bButton );
            }
            menu.OnMenuClose += sender =>
            {
                Game.Player.CanControlCharacter = true;
                new Thread( () =>
                {
                    Thread.Sleep( 5000 );
                    this._aroundSomeHouse = false;
                } )
                { Priority = ThreadPriority.Lowest }.Start();
            };
            menu.RefreshIndex();
            menu.Visible = true;
            this._menuPool.Add( menu );
        }

        private void CallDealer()
        {
            this.CreateApartmentsMenu();
        }
    }
}