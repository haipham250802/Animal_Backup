using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum TypeAnimalMinigame
{
    NONE = -1,
    CHO,
    RUA,
    CONG,
    RAN
}

public class UIminigame : MonoBehaviour
{
    public TypeAnimalMinigame Type;
    public AnimalMiniGameInfo Cho;
    public AnimalMiniGameInfo Rua;
    public AnimalMiniGameInfo Cong;
    public AnimalMiniGameInfo Ran;
    public Image IconAnimal;
    public Image Fill;
    public Button ExitButton;
    public BubleMinigame1[] bubles;

    public GameObject done;
    public GameObject wrong;

    public GameObject[] emojiCorrect;
    public GameObject[] emojiWrong;

    public static System.Action<bool> On_Check;
    public static System.Action On_dis;

    public GameObject[] hearts;
    public AudioSource Au;
    public AudioClip Correct;
    public AudioClip AWrong;
    public AudioClip Win;
    public AudioClip Lose;

    public float TimeCooldown;
    private float cooldown;
    int currentHeart = 3;
    int correctDone = 0;

    public PopupWin PopupWin;
    public PopupLose PopupLose;
    public void PlaySound(bool isCorrect)
    {
        if (isCorrect)
        {
            Au.clip = Correct;
        }
        else
            Au.clip = AWrong;
        Au.Play();
    }
    public void PlaySoundEnd(bool isWin)
    {
        if (isWin)
            Au.clip = Win;
        else
            Au.clip = Lose;
        Au.Play();
    }
    public void RemoveHeart()
    {
        currentHeart--;
        hearts[currentHeart].SetActive(false);
        if(currentHeart == 0)
        {
            ShowLose();
        }
    }
    private void StartDoFillEnd()
    {
        DOTween.To(() => 1f, _ =>
        {
            Fill.fillAmount = _;
        }, 0, TimeCooldown).OnComplete(()=> {
            ShowLose();
        });
    }
    public void Check(bool value)
    {
        ShowEmoji(value);
        /*    if (value)
                done.SetActive(true);
            else
                wrong.SetActive(true);*/
    }

    private void Disable()
    {
        done.SetActive(false);
        wrong.SetActive(false);
    }
    public void ShowEmoji(bool isCorrect)
    {
        int rand = 0;
        if (isCorrect)
        {
            rand = Random.Range(0, emojiCorrect.Length);
            emojiCorrect[rand].SetActive(true);
            correctDone++;
            if (correctDone >= 5)
            {
                DOVirtual.DelayedCall(1, () =>
                 {
                     PopupWin.gameObject.SetActive(true);
                     float value = Fill.fillAmount * 60;
                     Debug.Log("valuie:" + value );
                     int index = 0;
                     if (value <= 20)
                         index = 1;
                     else if (value <= 40)
                         index = 2;
                     else
                         index = 3;
                     PopupWin.InitStar(index);
                     PlaySoundEnd(true);
                 });
            }
        }
        else
        {
            rand = Random.Range(0, emojiWrong.Length);
            emojiWrong[rand].SetActive(true);
            RemoveHeart();
        }
        PlaySound(isCorrect);
    }
    private void ShowLose()
    {
        DOVirtual.DelayedCall(1, () =>
        {
            PopupLose.gameObject.SetActive(true);
            PlaySoundEnd(false);
        });
    }    
    public void DisableEmoji()
    {
        foreach (var item in emojiCorrect)
        {
            item.SetActive(false);
        }
        foreach (var item in emojiWrong)
        {
            item.SetActive(false);
        }
    }
    private void Start()
    {
        On_Check += Check;
        On_dis += Disable;
        StartDoFillEnd();
        int index = 0;
        int countWrong = 0;
        switch (Type)
        {
            case TypeAnimalMinigame.NONE:
                break;
            case TypeAnimalMinigame.CHO:
                for (int i = 0; i < bubles.Length; i++)
                {
                    if (i < Cho.Correct.Count)
                    {
                        bubles[i].desc.text = Cho.Correct[i];
                        bubles[i].isCorrect = true;
                        index++;
                    }
                }
                for (int i = index; i < bubles.Length; i++)
                {
                    bubles[i].desc.text = Cho.Wrong[countWrong];
                    countWrong++;
                }
                IconAnimal.sprite = Cho.AnimalShow;
                break;
            case TypeAnimalMinigame.RUA:
                for (int i = 0; i < bubles.Length; i++)
                {
                    if (i < Rua.Correct.Count)
                    {
                        bubles[i].desc.text = Rua.Correct[i];
                        bubles[i].isCorrect = true;
                        index++;
                    }
                }
                for (int i = index; i < bubles.Length; i++)
                {
                    bubles[i].desc.text = Rua.Wrong[countWrong];
                    countWrong++;
                }
                IconAnimal.sprite = Rua.AnimalShow;
                break;
            case TypeAnimalMinigame.CONG:
                for (int i = 0; i < bubles.Length; i++)
                {
                    if (i < Cong.Correct.Count)
                    {
                        bubles[i].desc.text = Cong.Correct[i];
                        bubles[i].isCorrect = true;
                        index++;
                    }
                }
                for (int i = index; i < bubles.Length; i++)
                {
                    bubles[i].desc.text = Cong.Wrong[countWrong];
                    countWrong++;
                }
                IconAnimal.sprite = Cong.AnimalShow;
                break;
            case TypeAnimalMinigame.RAN:
                for (int i = 0; i < bubles.Length; i++)
                {
                    if (i < Ran.Correct.Count)
                    {
                        bubles[i].desc.text = Ran.Correct[i];
                        bubles[i].isCorrect = true;
                        index++;
                    }
                }
                for (int i = index; i < bubles.Length; i++)
                {
                    bubles[i].desc.text = Ran.Wrong[countWrong];
                    countWrong++;
                }
                IconAnimal.sprite = Ran.AnimalShow;
                break;
            default:
                break;
        }
        ExitButton.onClick.AddListener(OnclickButtonExit);
    }
    private void OnclickButtonExit()
    {
      //  SceneManager.LoadScene(5);
    }
    private void OnDisable()
    {
        On_Check -= Check;
        On_dis -= Disable;
    }
}
[System.Serializable]
public class AnimalMiniGameInfo
{
    public TypeAnimalMinigame TypeAnimal;
    public Sprite AnimalShow;
    public List<string> Correct;
    public List<string> Wrong;
}
