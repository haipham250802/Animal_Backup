using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Animal : MonoBehaviour
{
    public List<EnvironmentType> validEnvironments;

    private Vector3 startPos;
    private bool isPlaced = false;
    private Camera cam;

    private SpriteRenderer sr;
    private int baseOrder;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;

        sr = GetComponent<SpriteRenderer>();
        baseOrder = sr.sortingOrder;
    }

    void Update()
    {
        if (isPlaced) return;

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();      // Mobile
#else
        HandleMouseInput();      // PC, Editor, WebGL
#endif
    }

    // --- PC / Editor / WebGL ---
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverAnimal(Input.mousePosition))
            {
                StartDrag();
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            DragTo(Input.mousePosition);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    // --- Mobile ---
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverAnimal(touch.position))
                {
                    StartDrag();
                }
            }
            else if (isDragging && touch.phase == TouchPhase.Moved)
            {
                DragTo(touch.position);
            }
            else if (isDragging &&
                     (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                EndDrag();
            }
        }
    }

    // --- Core drag logic ---
    void StartDrag()
    {
        if (isPlaced) return;
        isDragging = true;
        sr.sortingOrder = baseOrder + 1;
    }

    void DragTo(Vector2 screenPos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        transform.position = worldPos;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }

    void EndDrag()
    {
        isDragging = false;

        EnvironmentArea[] areas = FindObjectsOfType<EnvironmentArea>();
        float minDist = float.MaxValue;
        EnvironmentArea closest = null;

        foreach (var area in areas)
        {
            float dist = Vector3.Distance(transform.position, area.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = area;
            }
        }

        if (closest != null && validEnvironments.Contains(closest.environmentType))
        {
            isPlaced = true;
            closest.AddAnimal(this);
        }
        else
        {
            transform.DOMove(startPos, 0.3f).OnComplete(() =>
            {
                sr.sortingOrder = baseOrder;
            });
        }
    }

    public void MoveToCorrectEnvironment()
    {
        if (isPlaced) return;

        EnvironmentArea[] areas = FindObjectsOfType<EnvironmentArea>();
        foreach (var area in areas)
        {
            if (validEnvironments.Contains(area.environmentType))
            {
                isPlaced = true;
                area.AddAnimal(this);
                break;
            }
        }
    }

    // Kiểm tra click/chạm có đúng vào sprite con vật không
    bool IsPointerOverAnimal(Vector2 screenPos)
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(screenPos);
        Vector2 point2D = new Vector2(worldPoint.x, worldPoint.y);

        Collider2D hit = Physics2D.OverlapPoint(point2D);
        return hit != null && hit.gameObject == gameObject;
    }
}
