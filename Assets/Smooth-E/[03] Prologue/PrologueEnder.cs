using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrologueEnder : MonoBehaviour, IPointerDownHandler
{

    public FancyText bodyText;



    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<FancyText>().textSpelled)
        {
            PlayableSceneManager.levelIndex = 0;
            SceneManagerPlayable.LoadScene(1);
        }
    }
}
