using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BadBoyController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TextMeshProUGUI desc;
    private TextMeshProUGUI name;
    
    private void Start() {
        desc = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<TextMeshProUGUI>();
        name = GameObject.FindGameObjectWithTag("Character").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        name.text = "Valen";
        desc.text = "Fall in love with the bad boy";
    }

    public void OnPointerExit(PointerEventData eventData) {
        name.text = "";
        desc.text = "Select your romantic interest...";
    }
}
