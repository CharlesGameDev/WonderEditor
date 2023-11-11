using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AreaParamEditor : MonoBehaviour
{
    [SerializeField] TMP_InputField BgmType;
    [SerializeField] TMP_InputField EnvSetName;
    [SerializeField] TMP_InputField EnvironmentSound;
    [SerializeField] TMP_InputField EnvironmentSoundEfx;
    [SerializeField] Toggle IsNotCallWaterEnvSE;
    [SerializeField] TMP_InputField WonderBgmStartOffset;
    [SerializeField] TMP_InputField WonderBgmType;
    [SerializeField] Toggle DisableBgUnitDecoA;
    [SerializeField] TMP_InputField FieldA; 
    [SerializeField] TMP_InputField FieldB;
    [SerializeField] TMP_InputField Object;
    [SerializeField] TMP_InputField InitPaletteBaseName;
    [SerializeField] Transform paletteListParent;
    [SerializeField] TMP_InputField paletteListPrefab;

    public void UpdateValues()
    {
        AreaParam.Root r = AreaParamLoader.Instance.ap.root;
        
        BgmType.text = r.BgmType;
        EnvSetName.text = r.EnvSetName;
        EnvironmentSound.text = r.EnvironmentSound;
        EnvironmentSoundEfx.text = r.EnvironmentSoundEfx;
        IsNotCallWaterEnvSE.isOn = r.IsNotCallWaterEnvSE;
        WonderBgmStartOffset.text = r.WonderBgmStartOffset.ToString();
        WonderBgmType.text = r.WonderBgmType;
        InitPaletteBaseName.text = r.EnvPaletteSetting.InitPaletteBaseName;
        DisableBgUnitDecoA.isOn = r.SkinParam.DisableBgUnitDecoA;
        FieldA.text = r.SkinParam.FieldA;
        FieldB.text = r.SkinParam.FieldB;
        Object.text = r.SkinParam.Object;

        BgmType.onValueChanged.AddListener(value => AreaParamLoader.AP.root.BgmType = value);
        EnvSetName.onValueChanged.AddListener(value => AreaParamLoader.AP.root.EnvSetName = value);
        EnvironmentSound.onValueChanged.AddListener(value => AreaParamLoader.AP.root.EnvironmentSound = value);
        EnvironmentSoundEfx.onValueChanged.AddListener(value => AreaParamLoader.AP.root.EnvironmentSoundEfx = value);
        IsNotCallWaterEnvSE.onValueChanged.AddListener(value => AreaParamLoader.AP.root.IsNotCallWaterEnvSE = value);
        WonderBgmStartOffset.onValueChanged.AddListener(value => float.TryParse(value, out AreaParamLoader.AP.root.WonderBgmStartOffset));
        WonderBgmType.onValueChanged.AddListener(value => AreaParamLoader.AP.root.WonderBgmType = value);
        InitPaletteBaseName.onValueChanged.AddListener(value => AreaParamLoader.AP.root.EnvPaletteSetting.InitPaletteBaseName = value);
        DisableBgUnitDecoA.onValueChanged.AddListener(value => AreaParamLoader.AP.root.SkinParam.DisableBgUnitDecoA = value);
        FieldA.onValueChanged.AddListener(value => AreaParamLoader.AP.root.SkinParam.FieldA = value);
        FieldB.onValueChanged.AddListener(value => AreaParamLoader.AP.root.SkinParam.FieldB = value);
        Object.onValueChanged.AddListener(value => AreaParamLoader.AP.root.SkinParam.Object = value);

        for (int i = 0; i < paletteListParent.childCount; i++)
            Destroy(paletteListParent.GetChild(i).gameObject);

        if (r.EnvPaletteSetting.WonderPaletteList == null) r.EnvPaletteSetting.WonderPaletteList = new List<string>();

        for (int i = 0; i < r.EnvPaletteSetting.WonderPaletteList.Count; i++)
        {
            string item = r.EnvPaletteSetting.WonderPaletteList[i].Replace("Work/Gyml/Gfx/EnvPaletteParam/", "");
            item = item.Replace(".game__gfx__EnvPaletteParam.gyml", "");
            TMP_InputField field = Instantiate(paletteListPrefab, paletteListParent);
            field.text = item.ToString();

            field.onValueChanged.AddListener(value =>
            {
                SetPalette(i, value);
            });
        }
    }

    public void AddNewPalette()
    {
        AreaParamLoader.Instance.ap.root.EnvPaletteSetting.WonderPaletteList.Add("");
        UpdateValues();
    }

    public void SetPalette(int index, string value)
    {
        index--;
        AreaParamLoader.AP.root.EnvPaletteSetting.WonderPaletteList[index] = $"Work/Gyml/Gfx/EnvPaletteParam/{value}.game__gfx__EnvPaletteParam.gyml";
    }
}
