﻿#region Using

using System;
using GTA.Math;

#endregion

namespace BuyApartments.Model
{
    internal class Interior : IEquatable<Interior>
    {
        public Vector3 StartPoint { get; set; }
        public string Name { get; set; }

        #region Members

        public bool Equals( Interior other )
        {
            if ( ReferenceEquals( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals( this, other ) )
            {
                return true;
            }
            return this.StartPoint.Equals( other.StartPoint ) && string.Equals( this.Name, other.Name );
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
            return this.Equals( (Interior)obj );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ( this.StartPoint.GetHashCode() * 397 ) ^ ( this.Name != null ? this.Name.GetHashCode() : 0 );
            }
        }

        public static bool operator ==( Interior left, Interior right )
        {
            return Equals( left, right );
        }

        public static bool operator !=( Interior left, Interior right )
        {
            return !Equals( left, right );
        }

        #endregion

        public Interior( Vector3 startPoint, string name )
        {
            this.StartPoint = startPoint;
            this.Name = name;
        }
    }
}