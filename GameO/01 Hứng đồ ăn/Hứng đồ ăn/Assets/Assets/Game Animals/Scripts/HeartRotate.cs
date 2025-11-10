using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartRotate : MonoBehaviour
{
    [Header("Tốc độ xoay (độ/giây)")]
    public float rotateSpeed = 30f;

    void Update()
    {
        // Xoay quanh trục Y (từ trái qua phải)
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }
}
