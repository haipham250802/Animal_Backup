using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMinigameAll : MonoBehaviour
{
    [SerializeField] Button _minigame1;
    [SerializeField] Button _minigame2;
    [SerializeField] Button _minigame3;
    [SerializeField] Button _close;
    private void Start()
    {
        _minigame1.onClick.AddListener(OnClickButtonMinigame1);
        _minigame2.onClick.AddListener(OnClickButtonMinigame2);
        _minigame3.onClick.AddListener(OnClickButtonMinigame3);
        _close.onClick.AddListener(OnClickButtonClose);
    }
    private void OnClickButtonMinigame1()
    {
        SceneManager.LoadScene(4);
    }
    private void OnClickButtonMinigame2()
    {
        SceneManager.LoadScene(6);

    }
    private void OnClickButtonMinigame3()
    {
        SceneManager.LoadScene(7);

    }
    private void OnClickButtonClose()
    {
        SceneManager.LoadScene(2);

    }
}
