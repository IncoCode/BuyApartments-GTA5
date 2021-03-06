﻿#region Using

using System;
using BuyApartments.Model.JSON;
using GTA.Math;

#endregion

namespace BuyApartments.Model
{
    internal class Interior : IEquatable<Interior>
    {
        public Vector3 StartPoint { get; set; }
        public string Name { get; set; }
        public float Heading { get; set; }

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
            return this.Equals( (Interior)obj );
        }

        public override int GetHashCode()
        {
            return ( this.Name != null ? this.Name.GetHashCode() : 0 );
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

        #region Constructors

        public Interior( Vector3 startPoint, string name, float heading )
        {
            this.StartPoint = startPoint;
            this.Name = name;
            this.Heading = heading;
        }

        public Interior( CoordinatesJSON startPoint, string name, float heading ) :
            this( new Vector3( startPoint.X, startPoint.Y, startPoint.Z ), name, heading )
        {
        }

        #endregion

        public bool IsAroundStartPoint( Vector3 position, float range = 2 )
        {
            return range >= this.StartPoint.DistanceTo( position );
        }
    }
}