using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;       // player

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2f, -10f); // khoảng cách so với player
    public float smoothSpeed = 5f; // tốc độ mượt

    void LateUpdate()
    {
        if (target == null) return;

        // Chỉ follow Y và giữ X,Z
        Vector3 desiredPos = new Vector3(0, target.position.y, 0) + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPos;
    }
}
