#region Using

using System;
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
            return string.Equals( this.Name, other.Name ) && Equals( this.Interior, other.Interior ) &&
                   this.Price == other.Price && this.Location.Equals( other.Location );
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
            unchecked
            {
                int hashCode = ( this.Name != null ? this.Name.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( this.Interior != null ? this.Interior.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ this.Price;
                hashCode = ( hashCode * 397 ) ^ this.Location.GetHashCode();
                return hashCode;
            }
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

        public House( string name, Interior interior, int price, Vector3 location )
        {
            this.Name = name;
            this.Interior = interior;
            this.Price = price;
            this.Location = location;
        }
    }
}