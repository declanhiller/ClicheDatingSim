using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float targetScale = 1.02f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector2(targetScale, targetScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1, 1);

    }
}
