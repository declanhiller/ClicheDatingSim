using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float minSizeX;
    private float minSizeY;
    [SerializeField] private float sizeIncrease = 10;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        minSizeX = rectTransform.sizeDelta.x;
        minSizeY = rectTransform.sizeDelta.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.sizeDelta = new Vector2(minSizeX + sizeIncrease, minSizeY + sizeIncrease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.sizeDelta = new Vector2(minSizeX, minSizeY);

    }
}
