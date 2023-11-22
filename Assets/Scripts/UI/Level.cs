using System.Collections.Generic;

[System.Serializable]
public class Level
{
    public int Version;
    public bool IsBigEndian;
    public bool SupportPaths;
    public bool HasReferenceNodes;
    public Root root;
}

[System.Serializable]
public class Root
{
    public ActorToRailLink[] ActorToRailLinks;
    public List<Actor> Actors;
    public List<BgUnit> BgUnits;
    public Link[] Links;
    public Rail[] Rails;
    public uint RootAreaHash;
    public SimultaneousGroup[] SimultaneousGroups;
    public string StageParam;
}