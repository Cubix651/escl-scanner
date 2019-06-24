namespace Escl.Utils
{
    public struct Resolution
    {
        public int X {get; set;}
        public int Y {get; set;}

        public override string ToString()
        {
            return $"{X}x{Y}";
        }
    }
}