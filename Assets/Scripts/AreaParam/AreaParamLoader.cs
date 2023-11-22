using Nintendo.Byml;
using SFB;
using System.IO;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

public class AreaParamLoader : MonoBehaviour
{
    public static AreaParamLoader Instance { get; private set; }

    public AreaParam ap;
    public static AreaParam AP => Instance.ap;
    string filePath;
    [SerializeField] GameObject loadedDisable;
    [SerializeField] GameObject loadedEnable;
    [SerializeField] AreaParamEditor editor;

    private void Awake()
    {
        Instance = this;
        loadedDisable.SetActive(true);
        loadedEnable.SetActive(false);
    }

    public void OpenFile()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open .yaml or .bgyml", "", new ExtensionFilter[] { new(".yaml"), new(".bgyml") }, false);
        if (paths.Length == 0) return;
        filePath = paths[0];

        var yaml_content = "";
        if (filePath.EndsWith("yaml"))
            yaml_content = File.ReadAllText(filePath);
        else if (filePath.EndsWith(".bgyml"))
        {
            var data = File.ReadAllBytes(filePath);
            BymlFile file = new(data);
            yaml_content = file.ToYaml();
        }

        if (yaml_content == "") return;

        var deserializer = new DeserializerBuilder()
            .WithTagMapping("!ul", typeof(ulong))
            .WithTypeConverter(new ULongConverter())
            .WithTagMapping("!u", typeof(uint))
            .WithTypeConverter(new UIntConverter())
            .WithTagMapping("!l", typeof(long))
            .WithTypeConverter(new LongConverter())
            .Build();

        try
        {
            ap = deserializer.Deserialize<AreaParam>(yaml_content);
        } catch (YamlException)
        {
            ErrorPopup.Show("YamlException", "Invalid Area Param file.");
            return;
        }

        loadedDisable.SetActive(false);
        loadedEnable.SetActive(true);

        editor.UpdateValues();
    }

    public void Save()
    {
        if (ap == null) return;

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

        var yaml = serializer.Serialize(ap);
        if (yaml == null) return;
        File.WriteAllText(filePath.Replace(".yaml", ".new.yaml"), yaml);
    }
}