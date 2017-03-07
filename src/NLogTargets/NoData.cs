using System;
using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right")]
        [Obsolete("Operators are not supported, default implementation uses slow reflection.", true)]
        public static bool operator ==(NoData left, NoData right)
        {
            return true;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left")]
        [Obsolete("Operators are not supported, default implementation uses slow reflection.", true)]
        public static bool operator !=(NoData left, NoData right)
        {
            return false;
        }
    }
}