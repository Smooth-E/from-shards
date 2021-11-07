using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerPlayable : MonoBehaviour
{

    public static SceneManagerPlayable instance;
    public bool ableToLoad = false;

    void Start()
    {
        instance = this;
    }

    public IEnumerator waitTillAble(int buildIndex)
    {
        while (!ableToLoad) yield return null;
        SceneManager.LoadScene(buildIndex);
    }

    public static void LoadScene(int buildIndex)
    {
        DataManager.SaveData();
        instance.gameObject.GetComponent<Animator>().SetBool("isOpened", false);
        instance.StartCoroutine(instance.waitTillAble(buildIndex));
    }
}
