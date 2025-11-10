using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FoodType
{
    None,
    Worm,   // Giun
    Bone,   // Xương
    Fish,   // Cá
    Meat,   // Thịt
    Seed,   // Thóc
    Insect  // Sâu
}
[CreateAssetMenu(menuName = "Game/Animal Data")]
public class AnimalData : ScriptableObject
{
    public string animalName;
    public Sprite animalSprite;            // ảnh hiển thị (sprite của player)
    public List<FoodType> acceptableFoods; // đồ ăn có thể ăn
}
