using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public GameObject backGround;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Input.mousePosition;
        position = Camera.main.ScreenToWorldPoint(position);
        position.z = backGround.transform.position.z - 1.8f;
        transform.position = position;
    }
}
