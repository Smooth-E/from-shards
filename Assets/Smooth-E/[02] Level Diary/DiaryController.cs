using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour
{

    [System.Serializable]
    public class ItemCluster
    {
        public FancyText headerText, descriptionText;
        public Image iconImage;
    }

    int currentLevelIndex = 0;
    public FancyText headerText, descriptionText;
    public Image mainArtwork;
    public ItemCluster[] items = new ItemCluster[5];

    void Start()
    {
        DataManager.LoadData();
        currentLevelIndex = DataManager.data.level;
        UpdateStats();
    }

    void UpdateStats()
    {
        var levelData = DataManager.instance.levels[currentLevelIndex];
        headerText.ChangeText(levelData.chapterName);
        descriptionText.ChangeText(levelData.chapterStory);
        mainArtwork.sprite = levelData.mainArt;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var itemData = levelData.items[i];
            item.headerText.ChangeText(itemData.name);
            item.descriptionText.ChangeText(itemData.description);
            item.iconImage.sprite = itemData.icon;
        }
    }

    public void LeftArrow()
    {
        currentLevelIndex--;
        if (currentLevelIndex < 0) currentLevelIndex = DataManager.instance.levels.Length - 1;
        UpdateStats();
    }

    public void RightArrow()
    {
        currentLevelIndex++;
        if (currentLevelIndex > DataManager.instance.levels.Length - 1) currentLevelIndex = 0;
        UpdateStats();
    }
}
