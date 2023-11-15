using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    Vector3 lastPosition;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float currentZoom;
    [SerializeField] float zoomLerp;
    [SerializeField] float scrollSpeed;
    float rawZoom;
    Camera cam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();

        currentZoom = -cam.orthographicSize;
        rawZoom = currentZoom;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            float sens = mouseSensitivity * currentZoom;
            transform.Translate(delta.x * sens, delta.y * sens, 0);
            lastPosition = Input.mousePosition;
        }

        float scroll = Input.mouseScrollDelta.y * scrollSpeed;
        if (EventSystem.current.IsPointerOverGameObject())
            scroll = 0;

        rawZoom += scroll;
        if (rawZoom > -1)
            rawZoom = -1;

        currentZoom = Mathf.Lerp(currentZoom, rawZoom, zoomLerp * Time.deltaTime);

        // transform.position = new Vector3(transform.position.x, transform.position.y, currentZoom);
        cam.orthographicSize = -currentZoom;
    }
}
