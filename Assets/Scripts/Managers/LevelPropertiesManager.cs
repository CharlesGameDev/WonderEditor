using TMPro;
using UnityEngine;

public class LevelPropertiesManager : Manager
{
    [SerializeField] TMP_InputField rootAreaHash;

    public override void UpdateVisuals(Level level)
    {
        rootAreaHash.text = level.root.RootAreaHash.ToString();
    }
}
