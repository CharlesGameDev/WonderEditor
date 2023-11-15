using System;
using System.Collections.Generic;

[Serializable]
public class DynamicPropertyPreset
{
    [Serializable]
    public class DynamicProperty
    {
        public string name;
        public string value;
        public string type;
    }

    public string gyaml;
    public DynamicProperty[] properties;

    public Dictionary<string, object> ToDynamicDict()
    {
        Dictionary<string, object> dict = new();

        for (int i = 0; i < properties.Length; i++)
        {
            DynamicProperty p = properties[i];
            Type t = Type.GetType(p.type);
            object value;

            if (t == typeof(int))
                value = int.Parse(p.value);
            else if (t == typeof(bool))
                value = bool.Parse(p.value);
            else if (t == typeof(uint))
                value = uint.Parse(p.value);
            else if (t == typeof(long))
                value = long.Parse(p.value);
            else if (t == typeof(ulong))
                value = ulong.Parse(p.value);
            else if (t == typeof(float))
                value = float.Parse(p.value);
            else
                value = p.value;

            dict.Add(p.name, value);
        }

        return dict;
    }
}
