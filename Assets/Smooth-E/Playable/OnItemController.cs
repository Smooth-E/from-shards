using System.Collections;
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
    public Canvas canvas;
    string animatorBoolName = "isOpened";
    public RuntimeAnimatorController runtimeAnimatorController;

    // Start is called before the first frame update
    void Start()
    {
        runtimeAnimatorController = descriptionSnippet.GetComponent<Animator>().runtimeAnimatorController;
        canvas = Camera.main.transform.GetComponentInChildren<Canvas>();
        itemData = DataManager.instance.levels[PlayableSceneManager.levelIndex].items[id];
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        descriptionSnippet = Instantiate(descriptionSnippet, canvas.transform);
        StartCoroutine("ShowDescription");
    }

    IEnumerator ShowDescription()
    {
        while (true)
        {
            descriptionSnippet.GetComponent<Animator>().SetBool(animatorBoolName, hoveredWithMouse);
            if (hoveredWithMouse)
            {
                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                descriptionSnippet.GetComponent<RectTransform>().position = new Vector2(position.x, position.y);
            }
            yield return null;
        }
    }

    public void OnMouseEnter()
    {
        hoveredWithMouse = true;
    }

    public void OnMouseExit()
    {
        hoveredWithMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//короче, предлагаю оставить подсказки такими каие они есть, потому сделать их сложновато сделать )
