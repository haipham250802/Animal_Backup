using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalElement : MonoBehaviour
{
    public string name;
    public string name2;
    public Button _soundBtn;
    public AudioClip _sound;
    public AudioSource _aus;
    public string link;
   
    private void Start()
    {
    }
    private void OnEnable()
    {
        _soundBtn.onClick.RemoveAllListeners();
        _soundBtn.onClick.AddListener(OnClickButtonSound);
    }
    private void OnDisable()
    {
    }
    public void InitSound()
    {
        _aus.clip = _sound;
    }
    private void OnClickButtonSound()
    {
        _aus.Play();
    }
}
