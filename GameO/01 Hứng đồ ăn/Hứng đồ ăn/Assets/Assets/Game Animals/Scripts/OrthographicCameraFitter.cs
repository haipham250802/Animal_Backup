// OrthographicCameraFitter.cs
// Tác dụng: Tự điều chỉnh Camera Orthographic để khớp tỉ lệ màn hình.
// Tác giả: uc_ai + ChatGPT
// Gắn script này lên Camera (Orthographic).

using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class OrthographicCameraFitter : MonoBehaviour
{
    public enum FitMode
    {
        FitWidth,      // Đảm bảo "targetWorldWidth" luôn lọt vừa theo bề ngang
        FitHeight,     // Đảm bảo "targetWorldHeight" luôn lọt vừa theo bề dọc
        Contain,       // Chứa trọn (giống "fit" trong UI) -> lấy lớn hơn giữa FitWidth và FitHeight
        Cover          // Che phủ (giống "fill") -> lấy nhỏ hơn giữa FitWidth và FitHeight
    }

    [Header("Chế độ fit")]
    public FitMode fitMode = FitMode.Contain;

    [Header("Kích thước tham chiếu (đơn vị thế giới)")]
    [Tooltip("Chiều rộng cần đảm bảo luôn hiển thị đủ (FitWidth / Contain / Cover dùng).")]
    public float targetWorldWidth = 10f;
    [Tooltip("Chiều cao cần đảm bảo luôn hiển thị đủ (FitHeight / Contain / Cover dùng).")]
    public float targetWorldHeight = 10f;

    [Header("Cập nhật động")]
    [Tooltip("Tự cập nhật nếu thay đổi độ phân giải/tỉ lệ, kể cả khi đang chơi.")]
    public bool autoUpdate = true;

    [Header("Safe Area (tai thỏ, thanh điều hướng)")]
    [Tooltip("Áp safe area vào viewport của camera (di động).")]
    public bool applySafeArea = false;
    [Tooltip("Padding thêm (tỉ lệ 0..0.2 thường đủ).")]
    [Range(0f, 0.3f)] public float extraPadding = 0f;

    Camera cam;
    int lastScreenW, lastScreenH;
    Rect lastSafeArea;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("[OrthographicCameraFitter] Camera hiện không phải Orthographic. Đổi sang Orthographic để script hoạt động.");
            cam.orthographic = true;
        }

        ForceUpdateNow();
    }

    void Update()
    {
        if (!autoUpdate) return;

        bool resChanged = (Screen.width != lastScreenW) || (Screen.height != lastScreenH);
        bool safeChanged = applySafeArea && (Screen.safeArea != lastSafeArea);

        if (resChanged || safeChanged || !Application.isPlaying)
        {
            ForceUpdateNow();
        }
    }

    /// <summary>
    /// Gọi hàm này khi bạn thay đổi tham số bằng code/editor để cập nhật ngay.
    /// </summary>
    public void ForceUpdateNow()
    {
        if (cam == null) cam = GetComponent<Camera>();

        lastScreenW = Screen.width;
        lastScreenH = Screen.height;
        lastSafeArea = Screen.safeArea;

        // 1) Tính orthographicSize theo fit mode
        float aspect = (float)Screen.width / Mathf.Max(1, Screen.height);

        // size nếu muốn đảm bảo width: size = width / (2 * aspect)
        float sizeFitWidth = targetWorldWidth / (2f * Mathf.Max(0.0001f, aspect));
        // size nếu muốn đảm bảo height: size = height / 2
        float sizeFitHeight = targetWorldHeight * 0.5f;

        float chosenSize = cam.orthographicSize;

        switch (fitMode)
        {
            case FitMode.FitWidth:
                chosenSize = sizeFitWidth;
                break;
            case FitMode.FitHeight:
                chosenSize = sizeFitHeight;
                break;
            case FitMode.Contain:
                // Chứa trọn -> lấy max để chắc chắn cả chiều rộng & cao đều lọt
                chosenSize = Mathf.Max(sizeFitWidth, sizeFitHeight);
                break;
            case FitMode.Cover:
                // Che phủ -> lấy min để đảm bảo phủ kín (có thể cắt bớt cạnh)
                chosenSize = Mathf.Min(sizeFitWidth, sizeFitHeight);
                break;
        }

        // 2) Thêm padding (theo tỉ lệ) nếu muốn
        if (extraPadding > 0f)
        {
            chosenSize *= (1f + extraPadding);
        }

        cam.orthographicSize = Mathf.Max(0.0001f, chosenSize);

        // 3) Áp safe area (chỉnh camera.rect) nếu bật
        if (applySafeArea)
        {
            ApplySafeAreaToCameraRect();
        }
        else
        {
            cam.rect = new Rect(0, 0, 1, 1);
        }
    }

    void ApplySafeAreaToCameraRect()
    {
        Rect sa = Screen.safeArea;

        // Chuẩn hoá về 0..1 theo màn hình
        float x = sa.x / Screen.width;
        float y = sa.y / Screen.height;
        float w = sa.width / Screen.width;
        float h = sa.height / Screen.height;

        // Thêm padding nhỏ nếu muốn
        float padX = extraPadding * 0.05f; // scale nhẹ padding cho viewport
        float padY = extraPadding * 0.05f;

        x += padX;
        y += padY;
        w = Mathf.Clamp01(w - padX * 2f);
        h = Mathf.Clamp01(h - padY * 2f);

        cam.rect = new Rect(x, y, w, h);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (!cam.orthographic) return;

        // Vẽ khung tham chiếu theo kích thước target trong world
        Gizmos.color = Color.cyan;
        Vector3 center = cam.transform.position + cam.transform.forward * (cam.nearClipPlane + 1f);
        Vector3 size = new Vector3(targetWorldWidth, targetWorldHeight, 0f);
        Gizmos.DrawWireCube(new Vector3(cam.transform.position.x, cam.transform.position.y, center.z), size);

        // Vẽ khung nhìn hiện tại (theo orthographicSize & aspect)
        float aspect = (float)Screen.width / Mathf.Max(1, Screen.height);
        float halfH = cam.orthographicSize;
        float halfW = halfH * aspect;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(cam.transform.position.x, cam.transform.position.y, center.z),
            new Vector3(halfW * 2f, halfH * 2f, 0f));
    }
#endif
}
