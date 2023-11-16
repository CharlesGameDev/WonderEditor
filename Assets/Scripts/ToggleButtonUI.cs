using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool _isOn;
    [SerializeField] Image targetImage;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] UnityEvent<bool> onValueChanged;

    public bool isOn
    {
        get { return _isOn; }
        set {
            _isOn = value;
            UpdateSprite();
            onValueChanged.Invoke(isOn);
        }
    }

    void UpdateSprite()
    {
        targetImage.sprite = _isOn ? onSprite : offSprite;
    }

    private void Start()
    {
        UpdateSprite();
    }

    private void Reset()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isOn = !isOn;
    }
}
