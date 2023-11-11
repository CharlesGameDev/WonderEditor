using UnityEngine;
using UnityEngine.UI;

public class RainbowImage : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField, Range(0, 1)] float saturation = 1;
    [SerializeField, Range(0, 1)] float value = 1;
    [SerializeField, Range(1, 10)] float speed;

    private void Update()
    {
        image.color = Color.HSVToRGB((Time.time / speed) % 1f, saturation, value);
    }
}
