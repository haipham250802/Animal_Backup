using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIIntro1 : MonoBehaviour
{
    [SerializeField] Button _loadAnimals;
    [SerializeField] Button _loadMinigame;
    [SerializeField] Button _exit;
    private void Start()
    {
        _loadAnimals.onClick.AddListener(OnClickButtonLoadDV);
        _loadMinigame.onClick.AddListener(OnClickButtonLoadMiniGame);
        _exit.onClick.AddListener(OnClickButtonLoadExit);

    }
    private void OnClickButtonLoadDV()
    {
        SceneManager.LoadScene(3);
    }
    private void OnClickButtonLoadMiniGame()
    {
        SceneManager.LoadScene(5);
    }
    private void OnClickButtonLoadExit()
    {
        SceneManager.LoadScene(1);
    }
}
