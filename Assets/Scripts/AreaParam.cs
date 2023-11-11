using System.Collections.Generic;

[System.Serializable]
public class AreaParam
{
    [System.Serializable]
    public class Root
    {
        public string BgmType;
        public EnvPaletteSetting EnvPaletteSetting;
        public string EnvSetName;
        public string EnvironmentSound;
        public string EnvironmentSoundEfx;
        public bool IsNotCallWaterEnvSE;
        public SkinParam SkinParam;
        public float WonderBgmStartOffset;
        public string WonderBgmType;
    }

    public int Version;
    public bool IsBigEndian;
    public bool SupportPaths;
    public bool HasReferenceNodes;
    public Root root;
}

[System.Serializable]
public class EnvPaletteSetting
{
    public string InitPaletteBaseName;
    public List<string> WonderPaletteList;
}

[System.Serializable]
public class SkinParam
{
    public bool DisableBgUnitDecoA;
    public string FieldA;
    public string FieldB;
    public string Object;
}