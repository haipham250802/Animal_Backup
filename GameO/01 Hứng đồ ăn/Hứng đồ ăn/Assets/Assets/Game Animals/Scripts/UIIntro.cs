using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIIntro : MonoBehaviour
{
    [SerializeField] Button _loadintro2;
    private void Start()
    {
        _loadintro2.onClick.AddListener(OnClickButtonLoadintro2);
    }
    private void OnClickButtonLoadintro2()
    {
        SceneManager.LoadScene(1);
    }
}
