using System.Collections.Generic;
using UnityEngine;

public class ActorManager : Manager
{
    public static ActorManager Instance { get; private set; }

    [System.Serializable]
    public class ActorGroup
    {
        public string gyaml;
        public string group;
    }

    public Dictionary<string, Sprite> Sprites;
    public string[] ActorTypes;
    [SerializeField] bool[] ActorTypeVisibilityDefaults;
    public Dictionary<string, GameObject> ActorTypeObjects;
    public ActorGroup[] ActorGroups;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Sprites = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Icons"))
        {
            Sprites.Add(sprite.name, sprite);
        }
    }

    public void AddActor()
    {
        if (!LevelLoader.Instance.levelIsLoaded) return;

        ActorList.Instance.Show(AddActorCallback, "Add Actor");
    }
    public void AddActor(string gyaml) => AddActorCallback(gyaml);

    public void AddActorCallback(string gyaml)
    {
        if (!LevelLoader.Instance.levelIsLoaded) return;

        Actor newActor = new()
        {
            AreaHash = LevelLoader.Level.root.RootAreaHash,
            Name = LevelLoader.Level.root.Actors[0].Name + Random.Range(1, 999999),
            Hash = (ulong)Random.Range(0, ulong.MaxValue),
            Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray(),
            Scale = Vector3.one.ToArray(),
            Rotate = Vector3.zero.ToArray(),
            Layer = "PlayArea1",
            Gyaml = gyaml
        };
        newActor.Translate[2] = 0;

        foreach (DynamicPropertyPreset preset in ActorList.Instance.propertyPresets)
            if (preset.gyaml == gyaml)
                newActor.Dynamic = preset.ToDynamicDict();

        LevelLoader.Level.root.Actors.Add(newActor);
        UpdateVisuals(LevelLoader.Level);
    }

    public void DuplicateActor(Actor actor)
    {
        Actor newActor = new()
        {
            AreaHash = actor.AreaHash,
            Name = LevelLoader.Level.root.Actors[0].Name + Random.Range(1, 999999),
            Hash = (ulong)Random.Range(0, ulong.MaxValue),
            Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray(),
            Scale = actor.Scale,
            Rotate = actor.Rotate,
            Layer = actor.Layer,
            Gyaml = actor.Gyaml,
            Dynamic = actor.Dynamic,
            InLinks = actor.InLinks,
            System = actor.System
        };
        newActor.Translate[2] = actor.Translate[2];

        LevelLoader.Level.root.Actors.Add(newActor);
        UpdateVisuals(LevelLoader.Level);
    }

    public void DeleteActor(ActorView av)
    {
        LevelLoader.Level.root.Actors.Remove(av.actor);
        Destroy(av.gameObject);
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
            GameObject go = new(ActorList.GetActorName(actor.Gyaml));
            Transform parent = null;

            if (parent == null)
            {
                foreach (var item in ActorGroups)
                {
                    if (actor.Gyaml.StartsWith(item.gyaml))
                    {
                        foreach (var pair in ActorTypeObjects)
                        {
                            if (pair.Key == item.group)
                            {
                                parent = pair.Value.transform;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (parent == null)
            {
                foreach (var pair in ActorTypeObjects)
                {
                    if (actor.Gyaml.StartsWith(pair.Key))
                    {
                        parent = pair.Value.transform;
                        break;
                    }
                }
            }
            if (parent == null) parent = ActorTypeObjects["Other"].transform;
            go.transform.SetParent(parent);

            go.transform.localPosition = actor.Translate.ToVector3();
            go.transform.localScale = actor.Scale.ToVector3();
            go.transform.localRotation = actor.Rotate.ToRotation();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

            ActorView view = go.AddComponent<ActorView>();
            view.actor = actor;

            GameObject second = new("second");
            second.transform.SetParent(go.transform, false);
            SpriteRenderer sr2 = second.AddComponent<SpriteRenderer>();
            sr2.sortingOrder = sr.sortingOrder + 1;

            view.sr = sr;
            view.sr2 = sr2;
            view.UpdateSprite();

            PolygonCollider2D c = go.AddComponent<PolygonCollider2D>();
            c.isTrigger = true;
        }
    }
}
