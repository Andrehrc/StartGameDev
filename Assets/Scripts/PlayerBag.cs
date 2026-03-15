using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    [Header("Amounts")]
    public int totalWood;
    public float currentWater;
    public int carrots;
    public int fishes;


    [Header("Limits")]
    private float _limitWater = 30;
    private float _limitWood = 5;
    private float _limitCarrots = 10;
    private float _fishesLimit = 5;

    public float LimitWater { get => _limitWater; set => _limitWater = value; }
    public float LimitWood { get => _limitWood; set => _limitWood = value; }
    public float LimitCarrots { get => _limitCarrots; set => _limitCarrots = value; }
    public float FishesLimit { get => _fishesLimit; set => _fishesLimit = value; }

    public void AddWaterPlayerBag(float water)
    {
        currentWater += water;

        if (currentWater > _limitWater)
            currentWater = _limitWater;
    }
}
