#region Using

using System;
using BuyApartments.Model.JSON;
using GTA.Math;

#endregion

namespace BuyApartments.Model
{
    internal class House : IEquatable<House>
    {
        public string Name { get; set; }
        public Interior Interior { get; set; }
        public int Price { get; set; }
        public Vector3 Location { get; set; }
        public int DailyRent { get; set; }
        public Vector3 SavePoint { get; set; }

        #region Members

        public bool Equals( House other )
        {
            if ( ReferenceEquals( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals( this, other ) )
            {
                return true;
            }
            return string.Equals( this.Name, other.Name );
        }

        public override bool Equals( object obj )
        {
            if ( ReferenceEquals( null, obj ) )
            {
                return false;
            }
            if ( ReferenceEquals( this, obj ) )
            {
                return true;
            }
            if ( obj.GetType() != this.GetType() )
            {
                return false;
            }
            return this.Equals( (House)obj );
        }

        public override int GetHashCode()
        {
            return ( this.Name != null ? this.Name.GetHashCode() : 0 );
        }

        public static bool operator ==( House left, House right )
        {
            return Equals( left, right );
        }

        public static bool operator !=( House left, House right )
        {
            return !Equals( left, right );
        }

        #endregion

        #region Constructors

        public House( string name, Interior interior, int price, Vector3 location, int dailyRent, Vector3 savePoint )
        {
            this.Name = name;
            this.Interior = interior;
            this.Price = price;
            this.Location = location;
            this.DailyRent = dailyRent;
            this.SavePoint = savePoint;
        }

        public House( string name, Interior interior, int price, CoordinatesJSON location, int dailyRent,
            CoordinatesJSON savePoint )
            : this(
                name, interior, price, new Vector3( location.X, location.Y, location.Z ), dailyRent,
                new Vector3( savePoint.X, savePoint.Y, savePoint.Z ) )
        {
        }

        #endregion

        public bool IsInRange( Vector3 position, int range = 2 )
        {
            return range >= this.Location.DistanceTo( position );
        }
    }
}