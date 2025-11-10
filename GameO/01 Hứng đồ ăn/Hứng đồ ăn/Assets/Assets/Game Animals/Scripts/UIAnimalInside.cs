using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIAnimalInside : MonoBehaviour
{
    [SerializeField] Button _exit;
    private void Start()
    {
        _exit.onClick.AddListener(OnClickButtonExit);
    }
    private void OnClickButtonExit()
    {
        SceneManager.LoadScene(3);
    }
}
