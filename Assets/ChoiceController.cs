using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float targetScale = 1.02f;
    private float startScaleX;
    private float startScaleY;

    private void Start() {
        startScaleX = transform.localScale.x;
        startScaleY = transform.localScale.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("entering");
        transform.localScale = new Vector2(startScaleX * targetScale, startScaleY * targetScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("exiting");
        transform.localScale = new Vector2(startScaleX, startScaleY);

    }
}
