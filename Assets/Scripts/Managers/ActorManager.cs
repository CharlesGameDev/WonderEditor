using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ActorManager : Manager
{
    public static ActorManager Instance { get; private set; }

    public Dictionary<string, Sprite> Sprites;
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

    public void AddActor()
    {
        if (LevelLoader.Level == null) return;
        if (PopupField.Instance == null) return;

        PopupField.Instance.Show("Add Actor", "Actor GYAML", AddActorCallback);
    }

    public void AddActorCallback(string gyaml)
    {
        if (LevelLoader.Level == null) return;

        Actor newActor = new()
        {
            AreaHash = LevelLoader.Level.root.RootAreaHash,
            Name = LevelLoader.Level.root.Actors[0].Name + UnityEngine.Random.Range(1, 999999),
            Hash = (ulong)UnityEngine.Random.Range(0, ulong.MaxValue),
            Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray(),
            Scale = Vector3.one.ToArray(),
            Rotate = Vector3.zero.ToArray(),
            Layer = "PlayArea1",
            Gyaml = gyaml
        };
        newActor.Translate[2] = 0;

        LevelLoader.Level.root.Actors.Add(newActor);
        UpdateVisuals(LevelLoader.Level);
    }

    public void DuplicateActor(Actor actor)
    {
        Actor newActor = new()
        {
            AreaHash = actor.AreaHash,
            Name = LevelLoader.Level.root.Actors[0].Name + UnityEngine.Random.Range(1, 999999),
            Hash = (ulong)UnityEngine.Random.Range(0, ulong.MaxValue),
            Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray(),
            Scale = actor.Scale,
            Rotate = actor.Rotate,
            Layer = actor.Layer,
            Gyaml = actor.Gyaml
        };
        newActor.Translate[2] = actor.Translate[2];

        LevelLoader.Level.root.Actors.Add(newActor);
        UpdateVisuals(LevelLoader.Level);
    }

    public void DeleteActor(Actor actor)
    {
        LevelLoader.Level.root.Actors.Remove(actor);
        UpdateVisuals(LevelLoader.Level);
    }

    public override void UpdateVisuals(Level level)
    {
        if (ActorTypeObjects != null)
            foreach (var obj in ActorTypeObjects)
                Destroy(obj.Value);

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

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

            ActorView view = go.AddComponent<ActorView>();
            view.actor = actor;

            view.sr = sr;
            view.UpdateSprite();

            go.AddComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
