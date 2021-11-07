using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormController : MonoBehaviour
{

    int thunderSleepTime = 5;

    IEnumerator thunderCoroutine()
    {
        yield return new WaitForSeconds(thunderSleepTime);
        while (true)
        {
            if (Random.Range(0, 2) == 1) GetComponent<Animator>().SetTrigger("Thunder");
            yield return new WaitForSeconds(thunderSleepTime);
        }
    }

    public void Start()
    {
        StartCoroutine(thunderCoroutine());
    }
}
