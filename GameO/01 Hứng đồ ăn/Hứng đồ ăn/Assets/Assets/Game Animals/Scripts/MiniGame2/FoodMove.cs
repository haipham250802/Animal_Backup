using UnityEngine;

public class FoodMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Nếu rơi quá xa thì destroy
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
