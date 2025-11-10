using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 3f;
    public float horizontalSpeed = 10f;
    public float limitX = 3f;

    [Header("Animal Data")]
    public AnimalData currentAnimal;
    public SpriteRenderer spriteRenderer;  // gán vào SpriteRenderer của Player

    private Vector2 lastMousePos;
    private bool isDragging = false;

    public Transform FloatingPoint;
    Vector3 posFloatingOrigin;


    private void Start()
    {
        posFloatingOrigin = FloatingPoint.transform.localPosition;
    }
    void Update()
    {
        // Player luôn tiến lên
        transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);

#if UNITY_ANDROID || UNITY_IOS
    HandleTouch();
#else
        HandleMouse();   // Bao gồm cả WebGL
#endif

        // Giới hạn X
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        transform.position = pos;
    }

    public void FloatingPointUp()
    {
        FloatingPoint.transform.localPosition = posFloatingOrigin;
        FloatingPoint.gameObject.SetActive(true);
        FloatingPoint.transform.DOLocalMoveY(2.5f, 0.2f)
            .OnComplete(() =>
            {
                FloatingPoint.gameObject.SetActive(false);
            });
    }
    public void ChangeAnimal(AnimalData newAnimal)
    {
        currentAnimal = newAnimal;
        if (spriteRenderer != null && newAnimal.animalSprite != null)
        {
            spriteRenderer.sprite = newAnimal.animalSprite;
        }

        Debug.Log("Đã biến đổi thành: " + newAnimal.animalName);
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            float moveX = delta.x / Screen.width * horizontalSpeed * 20f;
            transform.Translate(Vector3.right * moveX * Time.deltaTime);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                lastMousePos = t.position;
                isDragging = true;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (isDragging && t.phase == TouchPhase.Moved)
            {
                Vector2 delta = t.position - lastMousePos;
                lastMousePos = t.position;

                float moveX = delta.x / Screen.width * horizontalSpeed * 20f;
                transform.Translate(Vector3.right * moveX * Time.deltaTime);
            }
        }
    }
}
