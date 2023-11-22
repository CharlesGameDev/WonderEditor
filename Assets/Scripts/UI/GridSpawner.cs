using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public Vector2 amount;
    List<Material> gridMats;

    private void Start()
    {
        GameObject prefab = transform.GetChild(0).gameObject;
        string mainS = PlayerPrefs.GetString("GridMain","#FFFFFF");
        string secondaryS = PlayerPrefs.GetString("GridSecondary", "#FFFFFF");
        string backgroundS = PlayerPrefs.GetString("GridBackground", "#000000");

        gridMats = new List<Material>();
        for (int x = 0; x < amount.x; x++)
        {
            for (int y = 0; y < amount.y; y++)
            {
                GameObject go = Instantiate(prefab, prefab.transform.position + (new Vector3(x, y) * 50), Quaternion.identity, transform);
                gridMats.Add(go.GetComponent<MeshRenderer>().material);
            }
        }

        if (ColorUtility.TryParseHtmlString(mainS, out Color main))
            SetMainColor(main);
        if (ColorUtility.TryParseHtmlString(secondaryS, out Color secondary))
            SetSecondaryColor(secondary);
        if (ColorUtility.TryParseHtmlString(backgroundS, out Color background))
            SetBackgroundColor(background);
    }

    public void SetMainColor(Color c)
    {
        if (gridMats == null) return;
        foreach (Material m in gridMats)
            m.SetColor("_MainColor", c);
    }

    public void SetSecondaryColor(Color c)
    {
        if (gridMats == null) return;
        foreach (Material m in gridMats)
            m.SetColor("_SecondaryColor", c);
    }

    public void SetBackgroundColor(Color c)
    {
        if (gridMats == null) return;
        foreach (Material m in gridMats)
            m.SetColor("_BackgroundColor", c);
    }
}
