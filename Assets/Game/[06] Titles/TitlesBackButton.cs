using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlesBackButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManagerPlayable.LoadScene(0);
    }
}