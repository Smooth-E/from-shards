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

    public static int currentLevelIndex = 0;
    public FancyText headerText, descriptionText;
    public Image mainArtwork;
    public ItemCluster[] items = new ItemCluster[5];
    public Sprite lockedItemSprite;

    void Start()
    {
        DataManager.LoadData();
        UpdateStats();
    }

    void UpdateStats()
    {
        if (DataManager.data.level >= currentLevelIndex)
        {
            var levelData = DataManager.instance.levels[currentLevelIndex];
            var o = headerText.gameObject;
            headerText.ChangeText(levelData.chapterName);
            headerText = o.GetComponent<FancyText>();
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
        else
        {
            var levelData = DataManager.instance.levels[currentLevelIndex];
            var o = headerText.gameObject;
            headerText.ChangeText("???");
            headerText = o.GetComponent<FancyText>();
            descriptionText.ChangeText("???");
            mainArtwork.sprite = levelData.mainArt;
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                var itemData = levelData.items[i];
                item.headerText.ChangeText("???");
                item.descriptionText.ChangeText("???");
                item.iconImage.sprite = lockedItemSprite;
            }
        }
    }

    public void LeftArrow()
    {
        currentLevelIndex--;
        if (currentLevelIndex < 0) currentLevelIndex = DataManager.instance.levels.Length - 1;
        SceneManagerPlayable.LoadScene(2);
    }

    public void RightArrow()
    {
        currentLevelIndex++;
        if (currentLevelIndex > DataManager.instance.levels.Length - 1) currentLevelIndex = 0;
        SceneManagerPlayable.LoadScene(2);
    }

    public void ButtonBack()
    {
        SceneManagerPlayable.LoadScene(0);
    }

    public void ButtonStart()
    {
        PlayableSceneManager.levelIndex = currentLevelIndex;
        SceneManagerPlayable.LoadScene(1);
    }
}
