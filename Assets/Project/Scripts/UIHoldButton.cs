using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool buttonHeld = false;
    public float holdTime = 0f;
    public float holdThreshold = 1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonHeld = true;
        holdTime = 0f;
    }

    // Implement the IPointerUpHandler interface
    public void OnPointerUp(PointerEventData eventData)
    {
        holdTime = 1f;
        buttonHeld = false;
    }

    void Update()
    {
        if (buttonHeld)
        {
            holdTime += Time.deltaTime;

        }
        else
        {
            if (holdTime > 0f)
            {
                holdTime -= Time.deltaTime;
            }
            else
            {
                holdTime = 0;
            }

        }
    }
}
