namespace BuyApartments.Model.JSON
{
    internal class CoordinatesJSON
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public CoordinatesJSON( float x, float y, float z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}