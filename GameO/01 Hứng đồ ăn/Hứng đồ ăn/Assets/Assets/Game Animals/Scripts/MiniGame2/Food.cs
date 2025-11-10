using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodType foodType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameControllerMinigame2>().OnEatFood(this);
            Destroy(gameObject);
        }
    }
}
