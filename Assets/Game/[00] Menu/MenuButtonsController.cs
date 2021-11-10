using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.LoadData();
    }

    public void ButtonMenu()
    {
        DiaryController.currentLevelIndex = DataManager.data.level;
        SceneManagerPlayable.LoadScene(2);
    }

    public void ButtonPlay()
    {
        var level = DataManager.data.level;
        if (level == 0) SceneManagerPlayable.LoadScene(3);
        else
        {
            PlayableSceneManager.levelIndex = level;
            SceneManagerPlayable.LoadScene(1);
        }
    }

    public void ButtonTitles()
    {
        SceneManagerPlayable.LoadScene(6);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
