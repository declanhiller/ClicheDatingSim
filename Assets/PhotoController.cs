using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image markerImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image photoImage;
    [SerializeField] private float fadeInSpeed = 2.5f;
    [SerializeField] private float fadeOutSpeed = 3;
    private bool canClick = false;


    private void Start()
    {
        markerImage.enabled = false;
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0);
        photoImage.color = new Color(photoImage.color.r, photoImage.color.g, photoImage.color.b, 0);
        StartCoroutine(FadeIn());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            markerImage.enabled = false;
            StartCoroutine(FadeOut());
            canClick = false;
        }
    }

    private IEnumerator FadeOut()
    {
        while (backgroundImage.color.a > 0)
        {
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, (backgroundImage.color.a - (fadeOutSpeed * Time.deltaTime)));
            photoImage.color = new Color(photoImage.color.r, photoImage.color.g, photoImage.color.b,
                (photoImage.color.a - (fadeOutSpeed * Time.deltaTime)));
            yield return new WaitForEndOfFrame();
        }
        GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().StartMenu();
    }

    private IEnumerator FadeIn()
    {
        while (backgroundImage.color.a < 1)
        {
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, (backgroundImage.color.a + (fadeInSpeed * Time.deltaTime)));
            photoImage.color = new Color(photoImage.color.r, photoImage.color.g, photoImage.color.b, (photoImage.color.a + (fadeInSpeed * Time.deltaTime)));
            yield return new WaitForEndOfFrame();
        }

        canClick = true;
        markerImage.enabled = true;
    }
}
