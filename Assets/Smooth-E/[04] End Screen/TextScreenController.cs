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
            if (level + 1 < 4) PlayableSceneManager.levelIndex = level + 1;
            else PlayableSceneManager.levelIndex = 3;
            SceneManagerPlayable.LoadScene(1);
        }
    }
}
