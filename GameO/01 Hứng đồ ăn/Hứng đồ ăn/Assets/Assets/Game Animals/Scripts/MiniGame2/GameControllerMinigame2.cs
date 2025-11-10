using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum EnvironmentType
{
    Land,
    Water,
    Sky,
    Both // cả nước và cạn
}
public class GameControllerMinigame2 : MonoBehaviour
{
    [Header("Animals")]
    public List<AnimalData> allAnimals;
    public PlayerController player;
    public GameObject finishLinePrefab;

    [Header("Food Settings")]
    public List<GameObject> foodPrefabs;
    public float spawnInterval = 2f;
    public float spawnDistance = 10f;
    public float laneOffset = 2f;
    public int maxSpawnRounds = 20;

    [Header("Animal Choice Settings")]
    public int choiceEveryRound = 5;
    public float animalSpawnDistance = 8f;
    public float animalLaneOffset = 3f;
    public Vector3 animalSpawnScale = Vector3.one;

    [Header("UI")]
    public UIHearts uiHearts;

    [Header("Gameplay")]
    public int maxWrong = 3;
    private int wrongCount = 0;
    private bool isGameOver = false;

    private int spawnCount = 0;

    // 🔥 biến để nhớ state sau AnimalChoice
    private bool forceCommonFoods = false;
    private List<FoodType> commonFoods = new List<FoodType>();
    private bool forceNextFoods = false;


    private AnimalData pendingAnimalA;
    private AnimalData pendingAnimalB;

    public Text Score;
    public GameObject PanelWin;
    public GameObject PanelLose;
    public int CountScore;

    private void Start()
    {
        if (uiHearts != null)
        {
            uiHearts.SetHearts(maxWrong - wrongCount, maxWrong);
        }
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (!isGameOver && spawnCount < maxSpawnRounds)
        {
            spawnCount++;

            if (spawnCount % choiceEveryRound == 0)
            {
                SpawnAnimalChoice();
            }
            else
            {
                SpawnFoods();
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        if (!isGameOver)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnFinishLine();
        }
    }
    public void OnReachFinish()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("YOU WIN!");
        Time.timeScale = 0f;
        PanelWin.gameObject.SetActive(true);
        Score.text = CountScore.ToString();
        // TODO: Show panel Win
    }
    private void SpawnFoods()
    {
        List<FoodType> foodsToSpawn = new List<FoodType>();

        if (forceNextFoods && pendingAnimalA != null && pendingAnimalB != null)
        {
            // ✅ món cho animal A
            FoodType foodA = pendingAnimalA.acceptableFoods[
                Random.Range(0, pendingAnimalA.acceptableFoods.Count)
            ];
            foodsToSpawn.Add(foodA);

            // ✅ món cho animal B (ưu tiên khác A, nhưng nếu ko có thì chấp nhận trùng)
            List<FoodType> choicesB = new List<FoodType>(pendingAnimalB.acceptableFoods);
            // loại bỏ những món đã có để tránh trùng
            choicesB.RemoveAll(f => foodsToSpawn.Contains(f));

            FoodType foodB;
            if (choicesB.Count > 0)
            {
                foodB = choicesB[Random.Range(0, choicesB.Count)];
            }
            else
            {
                // nếu ko có lựa chọn khác thì buộc phải trùng
                foodB = pendingAnimalB.acceptableFoods[Random.Range(0, pendingAnimalB.acceptableFoods.Count)];
            }
            foodsToSpawn.Add(foodB);

            // ✅ slot còn lại random bất kỳ, ưu tiên khác 2 cái trước
            FoodType randomFood;
            int maxTry = 10; // tránh loop vô hạn
            do
            {
                randomFood = (FoodType)Random.Range(1, System.Enum.GetValues(typeof(FoodType)).Length);
                maxTry--;
            } while (foodsToSpawn.Contains(randomFood) && maxTry > 0);

            foodsToSpawn.Add(randomFood);

            // reset flag
            forceNextFoods = false;
            pendingAnimalA = null;
            pendingAnimalB = null;
        }

        else
        {
            // ✅ Bình thường: ít nhất 1 đúng
            FoodType guaranteed = player.currentAnimal.acceptableFoods[
                Random.Range(0, player.currentAnimal.acceptableFoods.Count)
            ];
            foodsToSpawn.Add(guaranteed);

            while (foodsToSpawn.Count < 3)
            {
                bool spawnCorrect = (Random.value < 0.5f);
                FoodType type;
                if (spawnCorrect)
                {
                    type = player.currentAnimal.acceptableFoods[
                        Random.Range(0, player.currentAnimal.acceptableFoods.Count)
                    ];
                }
                else
                {
                    type = (FoodType)Random.Range(1, System.Enum.GetValues(typeof(FoodType)).Length);
                }

                if (!foodsToSpawn.Contains(type)) // 🔥 tránh trùng
                    foodsToSpawn.Add(type);
            }
        }

        // Shuffle vị trí
        for (int i = 0; i < foodsToSpawn.Count; i++)
        {
            int rand = Random.Range(i, foodsToSpawn.Count);
            (foodsToSpawn[i], foodsToSpawn[rand]) = (foodsToSpawn[rand], foodsToSpawn[i]);
        }

        // Spawn ra 3 lane
        for (int i = 0; i < 3; i++)
        {
            FoodType typeToSpawn = foodsToSpawn[i];
            GameObject prefab = foodPrefabs.Find(p =>
            {
                var f = p.GetComponent<Food>();
                return f != null && f.foodType == typeToSpawn;
            });

            if (prefab == null) continue;

            Vector3 pos = new Vector3((i - 1) * laneOffset, player.transform.position.y + spawnDistance, 0);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    private void SpawnAnimalChoice()
    {
        if (player == null || player.currentAnimal == null) return;

        List<AnimalData> candidates = new List<AnimalData>(allAnimals);
        candidates.RemoveAll(a => a == player.currentAnimal);

        if (candidates.Count < 2) return;

        AnimalData animalA = candidates[Random.Range(0, candidates.Count)];
        candidates.Remove(animalA);
        AnimalData animalB = candidates[Random.Range(0, candidates.Count)];

        float ySpawn = player.transform.position.y + animalSpawnDistance;

        SpawnAnimalPickup(animalA, new Vector3(-animalLaneOffset, ySpawn, 0));
        SpawnAnimalPickup(animalB, new Vector3(animalLaneOffset, ySpawn, 0));

        // 🔥 Lưu lại 2 con vật này để nhịp sau spawn food đúng
        pendingAnimalA = animalA;
        pendingAnimalB = animalB;
        forceNextFoods = true;
    }

    private void SpawnAnimalPickup(AnimalData data, Vector3 pos)
    {
        GameObject obj = new GameObject("AnimalPickup_" + data.animalName);
        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = data.animalSprite;
        var col = obj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        var pick = obj.AddComponent<AnimalPickup>();
        pick.animalData = data;

        sr.sortingOrder = 2;

        obj.transform.position = pos;
        obj.transform.localScale = animalSpawnScale;
    }

    private void SpawnFinishLine()
    {
        if (finishLinePrefab == null)
        {
            return;
        }

        Vector3 pos = new Vector3(finishLinePrefab.transform.position.x, player.transform.position.y + spawnDistance, 0);
        Instantiate(finishLinePrefab, pos, Quaternion.identity);
    }
    public void OnEatFood(Food food)
    {
        if (isGameOver) return;

        if (player.currentAnimal.acceptableFoods.Contains(food.foodType))
        {
            player.FloatingPointUp();
            CountScore += 10;
            // chỗ này bạn có thể cộng điểm hoặc hiệu ứng
        }
        else
        {
            wrongCount++;
            if (uiHearts != null)
            {
                uiHearts.SetHearts(maxWrong - wrongCount, maxWrong);
            }

            if (wrongCount >= maxWrong)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        // TODO: hiển thị panel thua, stop spawn loop...
        PanelLose.gameObject.SetActive(true);
        var arr = FindObjectsOfType<Food>();
        foreach (var item in arr)
        {
            item.gameObject.SetActive(false);
        }
    }
    public void ReloadScene()
    {
        Time.timeScale = 1;
        string nameScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nameScene);
    }
}

