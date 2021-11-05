using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsPanelController : MonoBehaviour
{

    [SerializeField] private float hideAfter = 1;
    Animator animator;
    bool touchedWithMouse = false;
    float timeOpened = 0;
    const string animatorBoolName = "isOpened";


    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    void Update()
    {
        if (!touchedWithMouse && animator.GetBool(animatorBoolName))
        {
            timeOpened += Time.deltaTime;
            if (timeOpened >= hideAfter) animator.SetBool(animatorBoolName, false);
        }
    }

    public void OnMouseEnter()
    {
        touchedWithMouse = true;
        animator.SetBool(animatorBoolName, true);
    }

    public void OnMouseExit()
    {
        touchedWithMouse = false;
        timeOpened = 0;
    }
}
