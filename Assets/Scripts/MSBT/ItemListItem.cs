using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemListItem : MonoBehaviour
{
    public Button button;
    public TMP_Text text;
    public string key;

    public void UpdateText()
    {
        text.text = key;
    }
}
