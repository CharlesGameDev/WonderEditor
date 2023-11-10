using System.Collections.Generic;

[System.Serializable]
public class Rail
{
    public uint AreaHash;
    public Dictionary<string, object> Dynamic;
    public string Gyaml;
    public ulong Hash;
    public bool IsClosed;
    public RailPoint[] Points;
}
