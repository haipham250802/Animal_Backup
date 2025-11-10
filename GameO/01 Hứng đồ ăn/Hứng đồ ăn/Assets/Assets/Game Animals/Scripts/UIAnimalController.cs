using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIAnimalController : MonoBehaviour
{
    [SerializeField] Button _can;
    [SerializeField] Button _nc;
    [SerializeField] Button _nc_can;
    [SerializeField] Button _troi;
    [SerializeField] Button _exit;

    [SerializeField] EviromentController _objCan;
    [SerializeField] EviromentController _objTroi;
    [SerializeField] EviromentController _objCannc;
    [SerializeField] EviromentController _objNc;


    private void Start()
    {
        _can.onClick.AddListener(OnClickButtonCan);
        _nc.onClick.AddListener(OnClickButtonNc);
        _nc_can.onClick.AddListener(OnClickButtonCanNc);
        _troi.onClick.AddListener(OnClickButtonTroi);
        _exit.onClick.AddListener(OnClickButtonExit);

        _objCan.gameObject.SetActive(false);
        _objTroi.gameObject.SetActive(false);
        _objCannc.gameObject.SetActive(false);
        _objNc.gameObject.SetActive(false);
    }
    private void OnClickButtonCan()
    {
        _objCan.gameObject.SetActive(true);
        _objTroi.gameObject.SetActive(false);
        _objCannc.gameObject.SetActive(false);
        _objNc.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
    private void OnClickButtonCanNc()
    {
        _objCan.gameObject.SetActive(false);
        _objTroi.gameObject.SetActive(false);
        _objCannc.gameObject.SetActive(true);
        _objNc.gameObject.SetActive(false);

        gameObject.SetActive(false);

    }
    private void OnClickButtonTroi()
    {
        _objCan.gameObject.SetActive(false);
        _objTroi.gameObject.SetActive(true);
        _objCannc.gameObject.SetActive(false);
        _objNc.gameObject.SetActive(false);

        gameObject.SetActive(false);

    }
    private void OnClickButtonNc()
    {
        _objCan.gameObject.SetActive(false);
        _objTroi.gameObject.SetActive(false);
        _objCannc.gameObject.SetActive(false);
        _objNc.gameObject.SetActive(true);

        gameObject.SetActive(false);

    }
    private void OnClickButtonExit()
    {
        SceneManager.LoadScene(2);
    }
}
