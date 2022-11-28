using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float speed = 0.2f;

    void Update()
    {
        Rect rect = image.uvRect;
        rect.x -= speed * Time.deltaTime;
        image.uvRect = rect;
    }
}
