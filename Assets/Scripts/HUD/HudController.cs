using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudController : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private Image waterUIBar;
    [SerializeField] private Image woodUIBar;
    [SerializeField] private Image carrotUIBar;
    [SerializeField] private Image fishUIBar;

    [Header("Tools")]
    public List<Image> toolsUI = new List<Image>();
    [SerializeField] private Color selectColor;
    [SerializeField] private Color alphaColor;

    PlayerBag bag;

    private void Awake()
    {
        bag = FindFirstObjectByType<PlayerBag>();
    }

    void Start()
    {
        ResetFillBars();
    }

    void Update()
    {
        waterUIBar.fillAmount = bag.currentWater / bag.LimitWater;
        woodUIBar.fillAmount = (float)bag.totalWood / bag.LimitWood;
        carrotUIBar.fillAmount = (float)bag.carrots / bag.LimitCarrots;
        fishUIBar.fillAmount = (float)bag.fishes / bag.FishesLimit;

    }

    public void UpdateToolUi(int selectedItem)
    {
        for (int i = 0; i < toolsUI.Count; i++)
        {
            if (i == selectedItem)
                toolsUI[i].color = selectColor;
            else
                toolsUI[i].color = alphaColor;
        }
    }

    public void ResetFillBars()
    {
        waterUIBar.fillAmount = 0;
        woodUIBar.fillAmount = 0;
        carrotUIBar.fillAmount = 0;
        fishUIBar.fillAmount = 0;
    }
}
