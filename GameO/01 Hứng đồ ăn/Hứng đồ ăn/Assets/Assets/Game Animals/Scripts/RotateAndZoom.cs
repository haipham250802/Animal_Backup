using UnityEngine;

[DisallowMultipleComponent]
public class RotateAndZoom : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    [Tooltip("Để trống sẽ dùng Camera.main")]
    public Camera controlCamera;

    [Header("Rotate (drag 1 ngón / chuột trái)")]
    public float rotateSpeed = 0.15f;
    public bool invertX = false;
    public bool invertY = false;
    [Range(0f, 1f)] public float rotationSmoothing = 0.12f;

    [Header("Zoom (scroll / pinch)")]
    public float maxScaleMultiplier = 3f;
    public float zoomSensitivity = 0.9f;
    public bool invertZoom = false;
    [Range(0f, 1f)] public float zoomSmoothing = 0.15f;

    Vector3 _initialScale;
    float _currentFactor = 1f;
    Quaternion _targetRotation;
    Vector3 _targetScale;

    void Awake()
    {
        if (!target) target = transform;
        if (!controlCamera) controlCamera = Camera.main;

        _initialScale = target.localScale;
        _targetRotation = target.rotation;
        _targetScale = target.localScale;
    }

    void Update()
    {
        HandleInput();

        // Apply smoothing
        if (rotationSmoothing > 0f)
            target.rotation = Quaternion.Slerp(
                target.rotation, _targetRotation,
                1f - Mathf.Pow(1f - rotationSmoothing, Time.deltaTime * 60f)
            );
        else
            target.rotation = _targetRotation;

        if (zoomSmoothing > 0f)
            target.localScale = Vector3.Lerp(
                target.localScale, _targetScale,
                1f - Mathf.Pow(1f - zoomSmoothing, Time.deltaTime * 60f)
            );
        else
            target.localScale = _targetScale;
    }

    void HandleInput()
    {
        // --- PC: chuột trái để xoay
        if (Input.GetMouseButton(0))
        {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");
            ApplyDragRotation(dx * 100f, dy * 100f);
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        // --- PC: scroll chuột để zoom
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.001f)
        {
            ApplyZoomDelta(scroll);
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        // --- Mobile: 1 ngón xoay
        if (Input.touchCount == 1)
        {
            Touch t0 = Input.GetTouch(0);
            Vector2 d = t0.deltaPosition;
            ApplyDragRotation(d.x, d.y);
        }

        // --- Mobile: 2 ngón pinch để zoom
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prev0 = t0.position - t0.deltaPosition;
            Vector2 prev1 = t1.position - t1.deltaPosition;

            float prevDist = (prev0 - prev1).magnitude;
            float currDist = (t0.position - t1.position).magnitude;

            float delta = currDist - prevDist; // >0 = kéo ra, <0 = kéo vào
            ApplyZoomDelta(delta * 0.01f);     // scale delta về nhỏ hơn
        }
#endif
    }

    void ApplyDragRotation(float dxPixels, float dyPixels)
    {
        if (!controlCamera) controlCamera = Camera.main;
        if (!controlCamera) return;

        // đảo chiều nếu user tick invert
        if (invertX) dxPixels = -dxPixels;
        if (invertY) dyPixels = -dyPixels;

        float yawDeg = dxPixels * rotateSpeed;
        float pitchDeg = -dyPixels * rotateSpeed;

        Vector3 camUp = controlCamera.transform.up;
        Vector3 camRight = controlCamera.transform.right;

        Quaternion qYaw = Quaternion.AngleAxis(yawDeg, camUp);
        Quaternion qPitch = Quaternion.AngleAxis(pitchDeg, camRight);

        _targetRotation = qYaw * qPitch * _targetRotation;
    }

    void ApplyZoomDelta(float delta)
    {
        if (invertZoom) delta = -delta;

        float desired = _currentFactor * (1f + delta * 0.1f * zoomSensitivity);
        _currentFactor = Mathf.Clamp(desired, 1f, Mathf.Max(1f, maxScaleMultiplier));
        _targetScale = _initialScale * _currentFactor;
    }
}
