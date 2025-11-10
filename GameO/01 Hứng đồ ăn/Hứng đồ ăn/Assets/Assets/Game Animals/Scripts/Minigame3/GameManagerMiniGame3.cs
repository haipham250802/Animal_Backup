using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMiniGame3 : MonoBehaviour
{
    public float timeLimit = 120f;   // tổng thời gian
    public Text timerText;

    public GameObject popupWin;
    public GameObject popupLose;
    public Image[] stars; // 3 cái Image (ẩn/hiện sao)

    private float timer;
    private bool isRunning = true;
    private bool gameEnded = false;

    void Start()
    {
        timer = timeLimit;

        if (popupWin != null) popupWin.SetActive(false);
        if (popupLose != null) popupLose.SetActive(false);
    }

    void Update()
    {
        if (!isRunning || gameEnded) return;

        timer -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timer).ToString();

        if (timer <= 0)
        {
            isRunning = false;
            CheckLose();
        }
        else
        {
            CheckWin();
        }
    }

    void CheckWin()
    {
        Animal[] animals = FindObjectsOfType<Animal>();
        foreach (var animal in animals)
        {
            if (!animal.IsPlaced()) // nếu còn con chưa đặt đúng
                return;
        }

        // Nếu đến đây nghĩa là tất cả đã đúng
        ShowWin();
    }

    void CheckLose()
    {
        Animal[] animals = FindObjectsOfType<Animal>();
        foreach (var animal in animals)
        {
            if (!animal.IsPlaced())
            {
                ShowLose();
                return;
            }
        }

        // Nếu hết giờ nhưng vẫn đặt đúng hết thì vẫn Win
        ShowWin();
    }

    void ShowWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (popupWin != null) popupWin.SetActive(true);

        int starCount = CalculateStars();
        ShowStars(starCount);
    }

    void ShowLose()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (popupLose != null) popupLose.SetActive(true);
    }

    int CalculateStars()
    {
        if (timer >= 80) return 3;
        if (timer >= 40) return 2;
        return 1;
    }

    void ShowStars(int count)
    {
        if (stars == null || stars.Length == 0) return;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < count);
        }
    }

    public void ReloadScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
