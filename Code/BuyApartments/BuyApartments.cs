#region Using

using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BuyApartments.Controller;
using BuyApartments.Model;
using GTA;
using GTA.Math;
using GTA.Native;
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
        private Vector3 _enterPoint;
        private float _enterHeading;
        private bool _canExitFromHouse = false;
        private Interior _currentInterior;

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
            Player player = Game.Player;
            House aroundHouse;
            if ( !this._aroundSomeHouse && this.AroundSomeHouse( out aroundHouse ) )
            {
                this._aroundSomeHouse = true;
                player.CanControlCharacter = false;
                this._blip?.Remove();
                this._blip = null;
                this.CreateHouseMenu( aroundHouse );
            }
            if ( this._currentInterior != null && this._currentInterior.IsAroundStartPoint( player.Character.Position ) && this._canExitFromHouse )
            {
                player.CanControlCharacter = false;
                this._currentInterior = null;
                //Game.FadeScreenIn( 500 );
                //Wait( 500 );
                this._aroundSomeHouse = true;
                player.Character.Position = this._enterPoint;
                float heading = this._enterHeading;
                player.Character.Heading = heading + 90 > 360 ? heading + 90 - 360 : heading + 90;
                //Game.FadeScreenOut( 500 );
                player.CanControlCharacter = true;
                this.ResetAroundSomeHouse( 10000 );
            }
        }

        private void ResetAroundSomeHouse( int delay = 5000 )
        {
            new Thread( () =>
            {
                Thread.Sleep( delay );
                this._aroundSomeHouse = false;
            } )
            { Priority = ThreadPriority.Lowest }.Start();
        }

        private void ResetCanExitFromHouse( int delay = 5000 )
        {
            new Thread( () =>
            {
                Thread.Sleep( delay );
                this._canExitFromHouse = true;
            } )
            { Priority = ThreadPriority.Lowest }.Start();
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
                    this._enterPoint = player.Character.Position;
                    this._canExitFromHouse = false;
                    this._currentInterior = boughtHouse.Interior;
                    //Game.FadeScreenIn( 500 );
                    //if ( Function.Call<bool>( Hash.IS_SCREEN_FADED_IN ) )
                    //{
                    //    Wait( 1 );
                    //}
                    //Wait( 500 );

                    Vector3 point = boughtHouse.Interior.StartPoint;
                    var interior = Function.Call<int>( Hash.GET_INTERIOR_AT_COORDS, point.X, point.Y, point.Z );
                    Function.Call( Hash.DISABLE_INTERIOR, interior, false );
                    Wait( 1000 );

                    player.Character.Position = point;
                    player.Character.Heading = boughtHouse.Interior.Heading;
                    //Game.FadeScreenOut( 500 );
                    //if ( Function.Call<bool>( Hash.IS_SCREEN_FADED_OUT ) )
                    //{
                    //    Wait( 1 );
                    //}
                    player.CanControlCharacter = true;
                    this.ResetCanExitFromHouse( 20000 );
                };
                menu.AddItem( bButton );
            }
            foreach ( House freeHouse in freeHouses )
            {
                var bButton = new UIMenuItem( "Buy " + freeHouse.Name );
                bButton.Activated += ( sender, item ) =>
                {
                    this._menuPool.CloseAllMenus();
                    Player player = Game.Player;
                    if ( player.Money < freeHouse.Price )
                    {
                        UI.Notify( "~r~Error:~r~ Not enough money!" );
                        return;
                    }
                    player.Money -= freeHouse.Price;
                    this._purchasedHousesController.BuyHouse( freeHouse );
                    this.ResetAroundSomeHouse( 2000 );
                };
                menu.AddItem( bButton );
            }
            menu.OnMenuClose += sender =>
            {
                Game.Player.CanControlCharacter = true;
                this.ResetAroundSomeHouse();
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