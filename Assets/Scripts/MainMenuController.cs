using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Referências")]
    public TMP_Text buttonText;
    public Image glowImage;
    public TMP_Text descriptionText;

    [Header("Descrição")]
    [TextArea]
    public string description;

    [Header("Texto")]
    public float normalSize = 32f;
    public float hoverSize = 40f;

    public Color normalColor = new Color(0.25f, 0.25f, 0.25f, 1f);
    public Color hoverColor = Color.white;

    [Header("Brilho")]
    public Color glowColor = new Color(1f, 1f, 1f, 0.25f);

    [Header("Animação")]
    public float animationSpeed = 10f;

    private bool isHovering = false;
    private Color glowHiddenColor;

    private void Start()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();

        if (glowImage != null)
        {
            glowHiddenColor = glowColor;
            glowHiddenColor.a = 0f;

            glowImage.color = glowHiddenColor;
        }

        if (buttonText != null)
        {
            buttonText.fontSize = normalSize;
            buttonText.color = normalColor;
        }
    }

    private void Update()
    {
        if (buttonText != null)
        {
            float targetSize = isHovering ? hoverSize : normalSize;
            Color targetColor = isHovering ? hoverColor : normalColor;

            buttonText.fontSize = Mathf.Lerp(buttonText.fontSize, targetSize, Time.unscaledDeltaTime * animationSpeed);
            buttonText.color = Color.Lerp(buttonText.color, targetColor, Time.unscaledDeltaTime * animationSpeed);
        }

        if (glowImage != null)
        {
            Color targetGlowColor = isHovering ? glowColor : glowHiddenColor;
            glowImage.color = Color.Lerp(glowImage.color, targetGlowColor, Time.unscaledDeltaTime * animationSpeed);
        }

        Vector3 targetScale = isHovering ? new Vector3(1.08f, 1.08f, 1f) : Vector3.one;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActivateHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeactivateHover();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ActivateHover();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DeactivateHover();
    }

    private void ActivateHover()
    {
        isHovering = true;

        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
    }

    private void DeactivateHover()
    {
        isHovering = false;
    }
}