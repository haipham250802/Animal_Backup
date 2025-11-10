using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UISellectMinigame1 : MonoBehaviour
{
    [SerializeField] Button _dogbutton;
    [SerializeField] Button _ruaButton;
    [SerializeField] Button _congButton;
    [SerializeField] Button _ranButton;
    [SerializeField] Button _exit;

    [SerializeField] UIminigame uiminigame;
    private void Start()
    {
        _dogbutton.onClick.AddListener(OnClickButtonDog);
        _ruaButton.onClick.AddListener(OnClickButtonRua);
        _congButton.onClick.AddListener(OnClickButtonCong);
        _ranButton.onClick.AddListener(OnClickButtonRan);
        _exit.onClick.AddListener(OnClickButtonDExit);
    }
    private void OnClickButtonDExit()
    {
        SceneManager.LoadScene(5);
    }
    private void OnClickButtonDog()
    {
        uiminigame.Type = TypeAnimalMinigame.CHO;
        uiminigame.gameObject.SetActive(true);
    }
    private void OnClickButtonRua()
    {
        uiminigame.Type = TypeAnimalMinigame.RUA;
        uiminigame.gameObject.SetActive(true);
    }
    private void OnClickButtonCong()
    {
        uiminigame.Type = TypeAnimalMinigame.CONG;
        uiminigame.gameObject.SetActive(true);
    }
    private void OnClickButtonRan()
    {
        uiminigame.Type = TypeAnimalMinigame.RAN;
        uiminigame.gameObject.SetActive(true);
    }
}
