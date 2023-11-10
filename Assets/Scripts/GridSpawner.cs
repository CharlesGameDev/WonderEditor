using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public Vector2 amount;

    private void Start()
    {
        GameObject prefab = transform.GetChild(0).gameObject;

        for (int x = 0; x < amount.x; x++)
        {
            for (int y = 0; y < amount.y; y++)
            {
                Instantiate(prefab, prefab.transform.position + (new Vector3(x, y) * 50), Quaternion.identity, transform);
            }
        }
    }
}
