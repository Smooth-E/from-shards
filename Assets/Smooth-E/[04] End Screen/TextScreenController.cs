using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextScreenController : MonoBehaviour, IPointerDownHandler
{
    public FancyText mainText, header;
    public Image image;

    public static int level = 0;

    public void Start()
    {
        var data = DataManager.instance.levels[level];
        mainText.ChangeText(data.chapterStory);
        header.ChangeText(data.chapterName);
        image.sprite = data.mainArt;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mainText.textSpelled)
        {
            if (level + 1 < 4) DiaryController.currentLevelIndex = level + 1;
            else DiaryController.currentLevelIndex = 3;
            SceneManagerPlayable.LoadScene(2);
        }
    }
}
