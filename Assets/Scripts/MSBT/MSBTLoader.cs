using Fushigi.Msbt;
using SFB;
using System.IO;
using UnityEngine;

public class MSBTLoader : MonoBehaviour
{
    public static MSBTLoader Instance { get; private set; }

    public MsbtFile msbt;
    public static MsbtFile MSBT => Instance.msbt;
    public string filePath;
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

        try
        {
            msbt = new MsbtFile(filePath);
        }
        catch (EndOfStreamException)
        {
            ErrorPopup.Show("EndOfStreamException", "Invalid MSBT file.");
            return;
        }
        loadedDisable.SetActive(false);
        loadedEnable.SetActive(true);

        editor.UpdateValues();
    }

    public void Save()
    {
        msbt.Save(filePath);
    }
}