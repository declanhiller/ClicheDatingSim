using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MilfController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI desc;
    private TextMeshProUGUI name;
    
    private void Start() {
        desc = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<TextMeshProUGUI>();
        name = GameObject.FindGameObjectWithTag("Character").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        name.text = "Euryale";
        desc.text = "Fall in love with the gorgon boss";
    }

    public void OnPointerExit(PointerEventData eventData) {
        name.text = "";
        desc.text = "Select your romantic interest...";
    }
}
