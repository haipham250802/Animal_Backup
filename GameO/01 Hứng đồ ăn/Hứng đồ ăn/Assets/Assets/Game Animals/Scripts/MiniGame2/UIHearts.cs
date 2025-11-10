using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHearts : MonoBehaviour
{
    public List<Image> hearts; // list trái tim (theo thứ tự từ trái sang phải)

    // hiển thị số mạng hiện tại
    public void SetHearts(int current, int max)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < current);
        }
    }
}
