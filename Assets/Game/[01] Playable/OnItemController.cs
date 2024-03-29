﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnItemController : MonoBehaviour
{

    public int id = 0;
    DataManager.Item itemData;
    bool hoveredWithMouse = false;
    static float waitToHideDescription = 0.5f;
    public GameObject descriptionSnippet;
    GameObject myDescriptionSnippet;
    public Canvas canvas;
    const string animatorBoolName = "isOpened";
    float itemSpeed = 10;


    public OnItemController(int id)
    {
        this.id = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = Camera.main.transform.GetComponentInChildren<Canvas>();

        myDescriptionSnippet = Instantiate(descriptionSnippet, canvas.transform);

        StartCoroutine(ShowDescription());
        StartCoroutine(rescaleOnstart());
    }

    IEnumerator ShowDescription()
    {
        while (true)
        {
            myDescriptionSnippet.GetComponent<Animator>().SetBool(animatorBoolName, hoveredWithMouse);
            if (hoveredWithMouse)
            {
                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                myDescriptionSnippet.GetComponent<RectTransform>().position = new Vector2(position.x, position.y);
            }
            yield return null;
        }
    }

    IEnumerator rescaleOnstart()
    {
        myDescriptionSnippet.SetActive(false);
        var fadingSpeed = 0.1f;
        for (float scale = 0; scale < 1; scale += fadingSpeed)
        {
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        myDescriptionSnippet.SetActive(true);
        transform.localScale = Vector3.one;
    }

    public void OnMouseEnter()
    {
        hoveredWithMouse = true;
    }

    public void OnMouseExit()
    {
        hoveredWithMouse = false;
    }

    void Update()
    {
        if (id != -1)
        {
            itemData = DataManager.instance.levels[PlayableSceneManager.levelIndex].items[id];
            GetComponent<SpriteRenderer>().sprite = itemData.icon;

            var t = myDescriptionSnippet.transform.GetChild(0).GetComponentsInChildren<FancyText>();
            t[0].textString = itemData.name;
            t[1].textString = itemData.description;
        }
        else
        {
            var t = myDescriptionSnippet.transform.GetChild(0).GetComponentsInChildren<FancyText>();
            t[0].textString = "Нет пути";
            t[1].textString = "\"Не могу. Какой - то блок\"";
        }


        int x = -1, y = -1;
        var field = PlayableSceneManager.instance.playField;
        for (int xIndex = 0; xIndex < field.GetLength(0); xIndex++)
        {
            for (int yIndex = 0; yIndex < field.GetLength(1); yIndex++)
            {
                if (Object.ReferenceEquals(field[xIndex, yIndex], this))
                {
                    x = xIndex;
                    y = yIndex;
                    break;
                }
            }
        }
        if (x != -1 && y != -1)
        {
            Vector2 cellPosition = PlayableSceneManager.instance.cellPositions[x, y];
            float step = 10 * Time.deltaTime;
            float diffX = transform.position.x - cellPosition.x;
            float diffY = transform.position.y - cellPosition.y;
            float newX, newY;
            if (step > Mathf.Abs(diffX)) newX = cellPosition.x;
            else newX = transform.position.x - step * (diffX / Mathf.Abs(diffX));
            if (step > Mathf.Abs(diffY)) newY = cellPosition.y;
            else newY = transform.position.y - step * (diffY / Mathf.Abs(diffY));
            transform.position = new Vector2(newX, newY);
        }

    }

    public void OnDestroy()
    {
        Destroy(myDescriptionSnippet);
    }

    IEnumerator moveTo(int x, int y)
    {
        while (true)
        {
            Vector2 cellPosition = PlayableSceneManager.instance.cellPositions[x, y];
            float step = 10 * Time.deltaTime;
            float diffX = transform.position.x - cellPosition.x;
            float diffY = transform.position.y - cellPosition.y;
            float newX, newY;
            if (step > Mathf.Abs(diffX)) newX = cellPosition.x;
            else newX = transform.position.x - step * (transform.position.x / Mathf.Abs(transform.position.x));
            if (step > Mathf.Abs(diffY)) newY = cellPosition.y;
            else newY = transform.position.y - step * (transform.position.y / Mathf.Abs(transform.position.y));
            transform.position = new Vector3(newX, newY, 2);
            if (new Vector2(transform.position.x, transform.position.y) == cellPosition) break;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator fade()
    {
        myDescriptionSnippet.SetActive(false);
        var fadingSpeed = 0.1f;
        for (float scale = 1; scale >= -fadingSpeed; scale -= fadingSpeed)
        {
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void FadeAndDestroy()
    {
        StartCoroutine(fade());
    }

    public void MoveAndDestroy(int x, int y)
    {
        StartCoroutine(moveTo(x, y));
    }
}
