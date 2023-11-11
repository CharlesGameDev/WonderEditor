using SFB;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    public Level level;
    public static Level Level => Instance.level;
    string filePath;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenFile()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open .yaml", "", "yaml", false);
        if (paths.Length == 0) return;

        filePath = paths[0];
        var yaml_content = File.ReadAllText(filePath);

        var deserializer = new DeserializerBuilder()
            .WithTagMapping("!ul", typeof(ulong))
            .WithTypeConverter(new ULongConverter())
            .WithTagMapping("!u", typeof(uint))
            .WithTypeConverter(new UIntConverter())
            .WithTagMapping("!l", typeof(long))
            .WithTypeConverter(new LongConverter())
            .Build();

        level = deserializer.Deserialize<Level>(yaml_content);

        GameManager.Instance.UpdateVisuals(level);
    }

    public void Save()
    {
        if (level == null) return;

        var serializer = new SerializerBuilder()
            .WithIndentedSequences()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithTagMapping("!ul", typeof(ulong))
            .WithTypeConverter(new ULongConverter())
            .WithTagMapping("!u", typeof(uint))
            .WithTypeConverter(new UIntConverter())
            .WithTagMapping("!l", typeof(long))
            .WithTypeConverter(new LongConverter())
            .EnsureRoundtrip()
            .Build();

        var yaml = serializer.Serialize(level);
        File.WriteAllText(filePath.Replace(".yaml", ".new.yaml"), yaml);
    }
}