using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MarkerController : MonoBehaviour {
    [SerializeField] private float minSize = 28;
    [SerializeField] private float maxSize = 50;
    [SerializeField] private float fadeSize = 0.5f;
    private RectTransform rectTransform;

    private bool isGoingUp;
    // Start is called before the first frame update
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rectTransform.rect.height <= minSize) {
            isGoingUp = true;
        } else if (rectTransform.rect.width >= maxSize) {
            isGoingUp = false;
        }

        if (isGoingUp) {
            float value = rectTransform.sizeDelta.x + (fadeSize * Time.deltaTime);
            rectTransform.sizeDelta = new Vector2(value, value);
        } else {
            float value = rectTransform.sizeDelta.x - (fadeSize * Time.deltaTime);
            rectTransform.sizeDelta = new Vector2(value, value);
        }
        
    }
}
