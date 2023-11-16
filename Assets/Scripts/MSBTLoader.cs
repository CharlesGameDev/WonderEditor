using MsbtEditor;
using Nintendo.Byml;
using SFB;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using ZstdSharp;

public class MSBTLoader : MonoBehaviour
{
    public static MSBTLoader Instance { get; private set; }

    public MSBT msbt;
    public static MSBT MSBT => Instance.msbt;
    string filePath;
    [SerializeField] GameObject loadedDisable;
    [SerializeField] GameObject loadedEnable;
    [SerializeField] MSBTEditor editor;

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

        msbt = new MSBT(filePath);

        loadedDisable.SetActive(false);
        loadedEnable.SetActive(true);

        editor.UpdateValues();
    }

    public void Save()
    {
        msbt.Save();
    }
}