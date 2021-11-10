using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLightController : MonoBehaviour
{
    public GameObject background;

    void Update()
    {
        Vector3 position = Input.mousePosition;
        position = Camera.main.ScreenToWorldPoint(position);
        position.z = background.transform.position.z - 1.8f;
        transform.position = position;
    }
}
