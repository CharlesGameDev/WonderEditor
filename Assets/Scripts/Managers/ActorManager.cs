using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ActorManager : Manager
{
    public static ActorManager Instance { get; private set; }

    Dictionary<string, Sprite> Sprites;
    public string[] ActorTypes;
    [SerializeField] bool[] ActorTypeVisibilityDefaults;
    public Dictionary<string, GameObject> ActorTypeObjects;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ILoadImages(Action<int, int, string> completedCallback)
    {
        Sprites = new Dictionary<string, Sprite>();

        string folderPath = Application.streamingAssetsPath + "/Icons";
        string[] filePaths = Directory.GetFiles(folderPath, "*.png");
        for (int i = 0; i < filePaths.Length; i++)
        {
            string path = filePaths[i];
            string name = Path.GetFileNameWithoutExtension(path);

            byte[] pngBytes = File.ReadAllBytes(path);
            Texture2D tex = new(2, 2);
            tex.LoadImage(pngBytes);

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.height);
            sprite.name = name;
            Sprites.Add(name, sprite);

            completedCallback.Invoke(i, filePaths.Length, name);

            yield return null;
        }

    }

    public override void UpdateVisuals(Level level)
    {
        ActorTypeObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < ActorTypes.Length; i++)
        {
            string type = ActorTypes[i];

            GameObject go = new(type);
            go.transform.SetParent(transform);

            ActorTypeObjects.Add(type, go);

            go.SetActive(ActorTypeVisibilityDefaults[i]);
        }
        foreach (Actor actor in level.root.Actors)
        {
            GameObject go = new($"{actor.Gyaml}:{actor.Name}");
            Transform parent = ActorTypeObjects["Other"].transform;
            foreach (var pair in ActorTypeObjects)
            {
                if (actor.Gyaml.StartsWith(pair.Key))
                {
                    parent = pair.Value.transform;
                    break;
                }
            }
            go.transform.SetParent(parent);

            go.transform.localPosition = actor.Translate.ToVector3();
            go.transform.localScale = actor.Scale.ToVector3();
            go.transform.localRotation = actor.Rotate.ToRotation();

            ActorView view = go.AddComponent<ActorView>();
            view.actor = actor;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            Sprite s = Sprites["unknown"];
            foreach (var sp in Sprites)
            {
                if (actor.Gyaml.Contains(sp.Value.name))
                {
                    s = sp.Value;
                    break;
                }
            }
            sr.sprite = s;

            go.AddComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
