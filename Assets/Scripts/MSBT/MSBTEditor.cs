using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class MSBTEditor : MonoBehaviour
{
    [SerializeField] Transform itemList;
    [SerializeField] GameObject itemListPrefab;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_InputField textInput;
    [SerializeField] TMP_Text previewText;

    public void UpdateValues()
    {
        infoText.text = Path.GetFileName(MSBTLoader.Instance.filePath);

        for (int i = 0; i < itemList.childCount; i++)
            Destroy(itemList.GetChild(i).gameObject);

        foreach (var item in MSBTLoader.MSBT.Messages.OrderBy(key => key.Key))
        {
            GameObject go = Instantiate(itemListPrefab, itemList);
            ItemListItem ili = go.GetComponent<ItemListItem>();
            ili.key = item.Key;
            ili.button.onClick.AddListener(() =>
            {
                ShowKey(item.Key);
            });

            ili.UpdateText();
        }
    }

    void ShowKey(string key)
    {
        string content = MSBTLoader.MSBT.Messages[key];
        
        textInput.onValueChanged.RemoveAllListeners();

        titleText.text = key;
        textInput.text = content;
        previewText.text = MakePreview(content);

        textInput.onValueChanged.AddListener(value =>
        {
            TextValueChanged(key, value);
        });
    }

    void TextValueChanged(string key, string value)
    {
        MSBTLoader.MSBT.Messages[key] = value;

        previewText.text = MakePreview(value);
    }

    string MakePreview(string content)
    {
        return content;
    }
}
