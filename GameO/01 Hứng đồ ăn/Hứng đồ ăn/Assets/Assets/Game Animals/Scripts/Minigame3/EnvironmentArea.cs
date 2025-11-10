using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnvironmentArea : MonoBehaviour
{
    public EnvironmentType environmentType;

    private List<Animal> animalsInArea = new List<Animal>();

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // gọi khi có 1 con vật đặt đúng chỗ
    public void AddAnimal(Animal animal)
    {
        if (!animalsInArea.Contains(animal))
        {
            animalsInArea.Add(animal);
            RearrangeAnimals();
        }
    }

    private void RearrangeAnimals()
    {
        // Tính toán layout dạng grid (vd: 2x2 hoặc 3x3 tuỳ số lượng)
        int count = animalsInArea.Count;
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(count)); // ví dụ 5 con → grid 3x3

        float spacing = 0.8f; // khoảng cách giữa các con
        Vector3 center = transform.position;

        for (int i = 0; i < count; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;

            // offset so cho grid nằm cân giữa tâm
            float offsetX = (col - (gridSize - 1) / 2f) * spacing;
            float offsetY = (row - (gridSize - 1) / 2f) * spacing;

            Vector3 targetPos = center + new Vector3(offsetX, offsetY, 0);

            animalsInArea[i].transform.DOMove(targetPos, 0.5f);
            animalsInArea[i].transform.DOScale(0.6f/5, 0.5f); // thu nhỏ lại cho gọn
        }
    }
}
