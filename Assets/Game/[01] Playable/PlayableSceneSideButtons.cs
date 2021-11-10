using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableSceneSideButtons : MonoBehaviour
{
    public void Again()
    {
        SceneManagerPlayable.LoadScene(1);
    }

    public void Menu()
    {
        SceneManagerPlayable.LoadScene(2);
    }
}
