using System;

namespace Nintendo.Aamp
{
    public class Curve
    {
        public uint NumUses { get; set; }
        public CurveType CurveType { get; set; }
        public float[] ValueFloats { get; set; } = Array.Empty<float>();
    }
}
