using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalSelectUI : MonoBehaviour
{
    [Header("References")]
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public PlayerController player;
    public GameControllerMinigame2 gameController;
    public List<AnimalData> allAnimals;

    [Header("Countdown")]
    public Text countdownText;      // UI text đếm ngược
    public float countdownTime = 3; // số giây chờ

    private void Start()
    {
        foreach (var animal in allAnimals)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
            Button btn = btnObj.GetComponent<Button>();

            Image img = btnObj.transform.Find("Icon").GetComponent<Image>();
            Text txt = btnObj.transform.Find("Text").GetComponent<Text>();

            btn.transform.localScale = Vector3.one * 0.3f;
            img.sprite = animal.animalSprite;
            img.SetNativeSize();
            txt.text = animal.animalName;

            btn.onClick.AddListener(() => OnSelectAnimal(animal));
        }
    }
    private void OnSelectAnimal(AnimalData animal)
    {
        // gán animal cho player
        player.ChangeAnimal(animal);
        gameController.player = player;

        // 🔥 Ẩn panel chọn animal, nhưng KHÔNG tắt object chứa script này
        buttonParent.gameObject.SetActive(false);

        // Bắt đầu countdown
        StartCoroutine(StartGameWithCountdown());
    }

    private IEnumerator StartGameWithCountdown()
    {
        countdownText.gameObject.SetActive(true);

        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        // 🔥 Bắt đầu game
        gameController.enabled = true;
    }

}
