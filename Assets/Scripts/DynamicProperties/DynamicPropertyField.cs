using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DynamicPropertyField : MonoBehaviour
{
    public TMP_InputField field1;
    public TMP_InputField field2Text;
    public Toggle field2Toggle;
    public TMP_Dropdown field2Dropdown;
    public UnityEvent onRemove;

    public void Remove()
    {
        onRemove.Invoke();
    }
}
