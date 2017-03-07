namespace NLogTargets
{
    public struct NoData
    {
        public static NoData Default { get; } = new NoData();
        
        public override bool Equals(object obj)
        {
            return obj is NoData;
        }

        public override int GetHashCode()
        {
            return 0;
        }
        
        public static bool operator ==(NoData left, NoData right)
        {
            return true;
        }

        public static bool operator !=(NoData left, NoData right)
        {
            return false;
        }
    }
}