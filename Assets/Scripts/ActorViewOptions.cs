using System.Linq;
using TMPro;
using UnityEngine;

public class ActorViewOptions : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.AddOptions(ActorManager.Instance.ActorTypes.ToList());
    }

    public void UpdateOptions()
    {
        Invoke(nameof(UpdateOptions_), 0.1f);
    }

    void UpdateOptions_()
    {
        ActorDropdownItem[] items = dropdown.GetComponentsInChildren<ActorDropdownItem>(false);

        foreach (ActorDropdownItem item in items)
        {
            item.toggle.interactable = true;
            item.toggle.isOn = ActorManager.Instance.ActorTypeObjects[item.label.text].activeSelf;  
            item.toggle.onValueChanged.AddListener(on =>
            {
                ActorManager.Instance.ActorTypeObjects[item.label.text].SetActive(on);
            });
        }
    }
}
