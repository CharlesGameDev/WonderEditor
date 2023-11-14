using UnityEngine;
using UnityEngine.UI;

public class TopButtons : MonoBehaviour
{
    [SerializeField] Button addActorButton;
    [SerializeField] Button addWallButton;
    [SerializeField] Button levelPropertiesButton;

    private static TopButtons instance;
    public static TopButtons Instance
    {
        get
        {
            if (instance == null) instance = FindFirstObjectByType<TopButtons>(FindObjectsInactive.Include);
            return instance;
        }
    }

    private void Update()
    {
        addActorButton.interactable = addWallButton.interactable = levelPropertiesButton.interactable = LevelLoader.Instance.levelIsLoaded;
    }
}
