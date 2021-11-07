using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourceController : MonoBehaviour
{

    public static SoundSourceController instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) Destroy(this.gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(soundUp());
    }

    IEnumerator soundUp()
    {
        GetComponent<AudioSource>().volume = 0;
        for (float i = 0; i < 1; i += 0.01f)
        {
            GetComponent<AudioSource>().volume = i;
            yield return null;
        }
        GetComponent<AudioSource>().volume = 1;
    }
}
