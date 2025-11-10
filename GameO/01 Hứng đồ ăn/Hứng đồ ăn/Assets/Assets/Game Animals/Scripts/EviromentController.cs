using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EviromentController : MonoBehaviour
{
    [SerializeField] AnimalElement[] animals;
    [SerializeField] AudioClip[] auClipThongtin;
    [SerializeField] AudioClip[] auClipMoiTruong;

    public Sprite[] BG;
    public Image BGImg;

    [SerializeField] Text _txt;
    [SerializeField] Text _name2Txt;
    [SerializeField] Button _next;
    [SerializeField] Button _prv;
    [SerializeField] Button _moitruong;
    [SerializeField] Button _back;
    [SerializeField] Button _kichthuoc;
    [SerializeField] Button _link;


    int index;
    public AudioSource Aus;
    private void Start()
    {

    }
    private void OnEnable()
    {
        index = 0;
        Active(index);
        InitName(animals[index].name);

        BGImg.sprite = BG[index];


        _next.onClick.RemoveAllListeners();
        _prv.onClick.RemoveAllListeners();
        _moitruong.onClick.RemoveAllListeners();
        _link.onClick.RemoveAllListeners();
     //   _back.onClick.RemoveAllListeners();
        _kichthuoc.onClick.RemoveAllListeners();

        _next.onClick.AddListener(OnClickNext);
        _prv.onClick.AddListener(OnClickPrv);
        _moitruong.onClick.AddListener(OnClickMoiTruong);
        _kichthuoc.onClick.AddListener(OnClickKichThoc);
     //   _back.onClick.AddListener(OnClickBack);
        _link.onClick.AddListener(OnCLikcLink);
    }
    private void OnCLikcLink()
    {
        Application.OpenURL(animals[index].link);
    }    
    private void OnClickBack()
    {
        BGImg.sprite = BG[index];
    }
    private void OnClickKichThoc()
    {
        Aus.clip = auClipThongtin[index];
        Aus.Play();
    }
    public void Force()
    {
        for (int i = 0; i < animals.Length; i++)
        {
            if (i != index)
                animals[i].gameObject.SetActive(false);
        }
        animals[index].gameObject.SetActive(true);
        animals[index].InitSound();
        InitName(animals[index].name);
        _name2Txt.text = animals[index].name2;
    }
    private void Active(int index)
    {
        for (int i = 0; i < animals.Length; i++)
        {
            if (i != index)
                animals[i].gameObject.SetActive(false);
        }
        animals[index].gameObject.SetActive(true);
        InitName(animals[index].name);
        animals[index].InitSound();
        _name2Txt.text = animals[index].name2;


    }
    private void OnClickNext()
    {
        index++;
        if (index > animals.Length - 1)
            index = 0;
        Active(index);
        InitName(animals[index].name);
        _name2Txt.text = animals[index].name2;
        BGImg.sprite = BG[index];
        animals[index].InitSound();
        Aus.Pause();
    }
    private void OnClickPrv()
    {
        index--;
        if (index < 0)
            index = animals.Length - 1;
        Active(index);
        InitName(animals[index].name);
        _name2Txt.text = animals[index].name2;
        BGImg.sprite = BG[index];
        animals[index].InitSound();
        Aus.Pause();
    }
    private void OnClickMoiTruong()
    {
        BGImg.gameObject.SetActive(true);
        BGImg.sprite = BG[index];

        Aus.clip = auClipMoiTruong[index];


        Aus.Play();
    }
    public void InitName(string name)
    {
        _txt.text = name;
    }
}
