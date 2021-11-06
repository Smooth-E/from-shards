using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Text))]
public class FancyText : MonoBehaviour, IPointerDownHandler
{

    Text textComponent;
    string textString;
    bool spellingCoroutineInProgress = false, textSpelled = false;
    [HideInInspector] public bool spellImidiately = false;
    const string spellingCoroutineName = "spellingCoroutine";
    const string setNiceTextSizeCoroutineName = "setNiceTextSizeCoroutine";
    public float spellingDelay = .01f;
    bool textSizeSet = false;

    void Start()
    {
        textComponent = GetComponent<Text>();
        textString = textComponent.text;
        StartCoroutine(setNiceTextSizeCoroutineName);
    }

    IEnumerator setNiceTextSizeCoroutine()
    {
        yield return null;
        int ceachedSize = (int)(textComponent.cachedTextGenerator.fontSizeUsedForBestFit / textComponent.canvas.scaleFactor);
        textComponent.resizeTextForBestFit = false;
        textComponent.fontSize = ceachedSize;
        textSizeSet = true;
    }

    IEnumerator spellingCoroutine()
    {
        spellingCoroutineInProgress = true;
        textComponent.text = "";
        foreach(char symbol in textString)
        {
            textComponent.text += symbol;
            if (spellImidiately)
            {
                textComponent.text = textString;
                break;
            }
            if (!isActiveAndEnabled) break;
            yield return new WaitForSeconds(spellingDelay);
        }
        textSpelled = true;
        spellingCoroutineInProgress = false;
    }

    void Update()
    {
        if (textSizeSet)
        {
            bool isActive = GetComponent<RectTransform>().lossyScale.x > 0 && GetComponent<RectTransform>().lossyScale.y > 0;
            Debug.Log(isActive);
            if (isActive && !spellingCoroutineInProgress && !textSpelled) StartCoroutine(spellingCoroutineName);
            else if (!isActive)
            {
                textComponent.text = "";
                textSpelled = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spellImidiately = true;
    }
}
