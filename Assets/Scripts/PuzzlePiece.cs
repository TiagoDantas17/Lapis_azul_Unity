using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private RectTransform rectTransform;

    private Canvas canvas;

    public Transform correctSlot;

    public bool isCorrect = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distance =
            Vector2.Distance(
                rectTransform.position,
                correctSlot.position);

        if (distance < 50)
        {
            rectTransform.position =
                correctSlot.position;

            isCorrect = true;
        }
    }
}