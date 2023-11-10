using TMPro;
using UnityEngine;

public class LevelPropertiesManager : Manager
{
    public static LevelPropertiesManager Instance { get; private set; }
    [SerializeField] TMP_InputField rootAreaHash;

    private void Awake()
    {
        Instance = this;
    }

    public override void UpdateVisuals(Level level)
    {
        rootAreaHash.text = level.root.RootAreaHash.ToString();
    }
}
