using System.Collections;
using System.Collections.Generic;
using Octokit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActorList : MonoBehaviour
{
    [System.Serializable]
    public class ActorEntry
    {
        public string name;
        public string gyaml;
    }

    [HideInInspector]
    public List<ActorEntry> actors;
    [SerializeField] TextAsset actorListText;
    [SerializeField] GameObject actorPrefab;
    [SerializeField] Transform actorListParent;

    void Start()
    {
        actors = new List<ActorEntry>();

        foreach (string s in actorListText.text.Split("\n"))
        {
            string[] parts = s.Split(",");
            ActorEntry ae = new ActorEntry()
            {
                name = parts[0].Trim(),
                gyaml = parts[1].Trim()
            };

            actors.Add(ae);
        }

        foreach (ActorEntry entry in actors)
        {
            GameObject go = Instantiate(actorPrefab, actorListParent);

            Transform c0 = go.transform.GetChild(0);
            Transform c1 = go.transform.GetChild(1);
            Transform c2 = go.transform.GetChild(2);

            c0.GetComponent<TMP_Text>().text = entry.name;
            c1.GetComponent<TMP_Text>().text = entry.gyaml;

            foreach (var pair in ActorManager.Instance.Sprites)
                if (entry.gyaml.Contains(pair.Key))
                    c2.GetComponent<Image>().sprite = pair.Value;
        }
    }
}
