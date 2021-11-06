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

    public OnItemController(int id)
    {
        this.id = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = Camera.main.transform.GetComponentInChildren<Canvas>();

        descriptionSnippet = Instantiate(descriptionSnippet, canvas.transform);

        StartCoroutine("ShowDescription");
    }

    IEnumerator ShowDescription()
    {
        while (true)
        {
            descriptionSnippet.GetComponent<Animator>().SetBool("isOpened", hoveredWithMouse);
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

    void Update()
    {
        itemData = DataManager.instance.levels[PlayableSceneManager.levelIndex].items[id];
        GetComponent<SpriteRenderer>().sprite = itemData.icon;

        var t = descriptionSnippet.transform.GetChild(0).GetComponentsInChildren<FancyText>();
        t[0].textString = itemData.name;
        t[1].textString = itemData.description;
    }
}
