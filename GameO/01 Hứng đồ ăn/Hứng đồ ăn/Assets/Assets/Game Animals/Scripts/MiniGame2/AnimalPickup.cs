using UnityEngine;

public class AnimalPickup : MonoBehaviour
{
    public AnimalData animalData; // con vật này đại diện

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeAnimal(animalData);
            }
            Destroy(gameObject); // biến mất sau khi chọn
        }
    }
}
