using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class PopupHint : MonoBehaviour
{
    [SerializeField] private float appearanceDistance = 1.0f;
    [SerializeField] private float journeyTime;
    private SpriteRenderer sprite;
    private bool stopAnimate = false;

    private void Start()
    {
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Hide();
    }
    public void StopAnimation()
    {
        stopAnimate = true;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void StartAnimation()
    {
        stopAnimate = false;
        StartCoroutine(AnimateAppearance());
    }
    private IEnumerator AnimateAppearance()
    {
        transform.localPosition = Vector3.zero;
        float startTime = Time.time;
        float fracComplete = (Time.time - startTime) / journeyTime;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position;
        startPosition.y -= appearanceDistance;

        float r = sprite.color.r;
        float g = sprite.color.g;
        float b = sprite.color.b;
        Color startColor = new Color(r, g, b, 0);
        sprite.color = startColor;

        while (fracComplete <= 1)
        {
            if (stopAnimate)
                break;
            fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, fracComplete);

            float alpha = Mathf.Lerp(startColor.a,1, fracComplete);

            sprite.color = new Color(r,g,b,alpha);
          
            yield return null;
        }
    }
}
