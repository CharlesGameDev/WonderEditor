using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Manager[] managers;
    
    private void Awake()
    {
        Instance = this;
    }

    public void UpdateVisuals(Level level)
    {
        foreach (var manager in managers)
            manager.UpdateVisuals(level);
    }
}
