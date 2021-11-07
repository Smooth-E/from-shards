using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostScreenController : MonoBehaviour
{
    public static int level = 0;
    
    public void Again()
    {
        PlayableSceneManager.levelIndex = level;
        SceneManagerPlayable.LoadScene(1);
    }

    public void Menu()
    {
        SceneManagerPlayable.LoadScene(0);
    }
}
