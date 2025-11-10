using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class BubleMinigame1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Setup")]
    public Text desc;
    public bool isCorrect = false;

    [Header("Drag Config")]
    [Tooltip("Canvas chứa các UI của minigame (bắt buộc).")]
    public Canvas canvas;
    [Tooltip("RectTransform của icon Animals để kiểm tra khoảng cách.")]
    public RectTransform dropTarget;
    [Tooltip("Khoảng cách (pixel trên màn hình) để tính là 'gần' iconAnimals.")]
    public float triggerScreenDistance = 120f;
    [Tooltip("Kéo xong có quay về vị trí ban đầu không?")]
    public bool returnToStart = true;

    private RectTransform rect;
    private Vector2 startAnchoredPos;
    private CanvasGroup canvasGroup;

    public GameObject done;
    public GameObject wrong;

    public bool IsDwestroyDrop;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        // Lưu vị trí ban đầu để snap-back
        startAnchoredPos = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Cho phép drag đi qua các raycast khác (nếu có)
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        // Chuyển vị trí screen → local trong canvas để kéo mượt
        RectTransform canvasRect = canvas.transform as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            rect.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (dropTarget != null)
        {
            // So sánh khoảng cách trên SCREEN để ổn định với mọi render mode
            Vector2 bubbleScreenPos = RectTransformUtility.WorldToScreenPoint(null, rect.position);
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(null, dropTarget.position);
            float dist = Vector2.Distance(bubbleScreenPos, targetScreenPos);

            // Nếu kéo đủ gần iconAnimals thì log theo isCorrect
            if (dist <= triggerScreenDistance)
            {
                Debug.Log(isCorrect ? "true" : "false");
                StartCoroutine(IE_delayDrop(isCorrect));
                return;
            }
        }

        if (returnToStart)
        {
            rect.anchoredPosition = startAnchoredPos;
        }
    }
    IEnumerator IE_delayDrop(bool value)
    {
        UIminigame.On_Check?.Invoke(value);
        yield return new WaitForSeconds(0.2f);
        UIminigame.On_dis?.Invoke();
        if (IsDwestroyDrop)
            gameObject.SetActive(false);
    }
}
