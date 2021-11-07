using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public GameObject rain;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(RainCoroutine());
    }
    IEnumerator RainCoroutine()
    {
        while(true)
        {
            Vector3 topOfScreen = new Vector3(Random.Range(0, (float)Screen.width), Screen.height, 1);
            Instantiate(rain, cam.ScreenToWorldPoint(topOfScreen), Quaternion.identity);

            yield return new WaitForSeconds(0.1f);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
