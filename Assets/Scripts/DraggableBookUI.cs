using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBookUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BookshelfOrderPuzzle puzzleManager;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;

        Debug.Log("Livro largado.");

        if (puzzleManager == null)
        {
            puzzleManager = GetComponentInParent<BookshelfOrderPuzzle>();
        }

        if (puzzleManager != null)
        {
            puzzleManager.ResolverPuzzleDireto();
        }
        else
        {
            Debug.LogWarning("Não encontrei o BookshelfOrderPuzzle.");
        }
    }
}