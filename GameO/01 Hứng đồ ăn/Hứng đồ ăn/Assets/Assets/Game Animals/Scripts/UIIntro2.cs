using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;


public class UIIntro2 : MonoBehaviour
{
    [SerializeField] Button _load;
    [SerializeField] Button _exit;
    private void Start()
    {
        _load.onClick.AddListener(OnClickButtonLoad);
        _exit.onClick.AddListener(OnClickButtonLoadExit);
    }
    private void OnClickButtonLoad()
    {
        SceneManager.LoadScene(2);
    }
    private void OnClickButtonLoadExit()
    {
        SceneManager.LoadScene(0);
    }
}
