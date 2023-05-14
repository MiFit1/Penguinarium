using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Framing : MonoBehaviour
{
    [SerializeField] private float journeyTime = 1f;
    [SerializeField] private GameObject StartFrame;
    [SerializeField] private GameObject FinishFrame;

    [SerializeField] private GameObject LeftUpCorner;
    [SerializeField] private GameObject RighUpCorner;
    [SerializeField] private GameObject RightBottomCorner;
    [SerializeField] private GameObject LeftBottomCorner;

    private bool stopAnimate = false;

    private SpriteRenderer LeftUpSprite;
    private SpriteRenderer RightUpSprite;
    private SpriteRenderer RightBottomSprite;
    private SpriteRenderer LeftBottomSprite;
    private void Start()
    {
        StartFrame.GetComponent<SpriteRenderer>().enabled = false;
        FinishFrame.GetComponent<SpriteRenderer>().enabled = false;

        LeftUpSprite = LeftUpCorner.GetComponent<SpriteRenderer>();
        RightUpSprite = RighUpCorner.GetComponent<SpriteRenderer>();
        RightBottomSprite = RightBottomCorner.GetComponent<SpriteRenderer>();
        LeftBottomSprite = LeftBottomCorner.GetComponent<SpriteRenderer>();

        LeftUpSprite.enabled = false;
        RightUpSprite.enabled = false;
        RightBottomSprite.enabled = false;
        LeftBottomSprite.enabled = false;
    }

    public void StopAnimation()
    {
        stopAnimate = true;
        LeftUpSprite.enabled = false;
        RightUpSprite.enabled = false;
        RightBottomSprite.enabled = false;
        LeftBottomSprite.enabled = false;
    }

    public void StartAnimation()
    {
        stopAnimate = false;
        LeftUpSprite.enabled = true;
        RightUpSprite.enabled = true;
        RightBottomSprite.enabled = true;
        LeftBottomSprite.enabled = true;

        Vector2 LeftUpStartFrame = Vector2.zero;
        LeftUpStartFrame.x = (StartFrame.transform.localScale.x / 2f) * (-1);
        LeftUpStartFrame.y = (StartFrame.transform.localScale.y / 2f);
        LeftUpStartFrame.x += StartFrame.transform.localPosition.x;
        LeftUpStartFrame.y += StartFrame.transform.localPosition.y;

        Vector2 RightUpStartFrame = Vector2.zero;
        RightUpStartFrame.x = (StartFrame.transform.localScale.x / 2f);
        RightUpStartFrame.y = (StartFrame.transform.localScale.y / 2f);
        RightUpStartFrame.x += StartFrame.transform.localPosition.x;
        RightUpStartFrame.y += StartFrame.transform.localPosition.y;

        Vector2 RightBottomStartFrame = Vector2.zero;
        RightBottomStartFrame.x = (StartFrame.transform.localScale.x / 2f);
        RightBottomStartFrame.y = (StartFrame.transform.localScale.y / 2f) * (-1);
        RightBottomStartFrame.x += StartFrame.transform.localPosition.x;
        RightBottomStartFrame.y += StartFrame.transform.localPosition.y;

        Vector2 LeftBottomStartFrame = Vector2.zero;
        LeftBottomStartFrame.x = (StartFrame.transform.localScale.x / 2f) * (-1);
        LeftBottomStartFrame.y = (StartFrame.transform.localScale.y / 2f) * (-1);
        LeftBottomStartFrame.x += StartFrame.transform.localPosition.x;
        LeftBottomStartFrame.y += StartFrame.transform.localPosition.y;


        Vector2 LeftUpFinishFrame = Vector2.zero;
        LeftUpFinishFrame.x = (FinishFrame.transform.localScale.x / 2f) * (-1);
        LeftUpFinishFrame.y = (FinishFrame.transform.localScale.y / 2f);
        LeftUpFinishFrame.x += FinishFrame.transform.localPosition.x;
        LeftUpFinishFrame.y += FinishFrame.transform.localPosition.y;

        Vector2 RightUpFinishFrame = Vector2.zero;
        RightUpFinishFrame.x = (FinishFrame.transform.localScale.x / 2f);
        RightUpFinishFrame.y = (FinishFrame.transform.localScale.y / 2f);
        RightUpFinishFrame.x += FinishFrame.transform.localPosition.x;
        RightUpFinishFrame.y += FinishFrame.transform.localPosition.y;

        Vector2 RightBottomFinishFrame = Vector2.zero;
        RightBottomFinishFrame.x = (FinishFrame.transform.localScale.x / 2f);
        RightBottomFinishFrame.y = (FinishFrame.transform.localScale.y / 2f) * (-1);
        RightBottomFinishFrame.x += FinishFrame.transform.localPosition.x;
        RightBottomFinishFrame.y += FinishFrame.transform.localPosition.y;

        Vector2 LeftBottomFinishFrame = Vector2.zero;
        LeftBottomFinishFrame.x = (FinishFrame.transform.localScale.x / 2f) * (-1);
        LeftBottomFinishFrame.y = (FinishFrame.transform.localScale.y / 2f) * (-1);
        LeftBottomFinishFrame.x += FinishFrame.transform.localPosition.x;
        LeftBottomFinishFrame.y += FinishFrame.transform.localPosition.y;

        StartCoroutine(Animation(LeftUpStartFrame, RightUpStartFrame, RightBottomStartFrame, LeftBottomStartFrame, 
            LeftUpFinishFrame, RightUpFinishFrame, RightBottomFinishFrame, LeftBottomFinishFrame));
    }

    private IEnumerator Animation(Vector2 leftUpStart, Vector2 RightUpStart, Vector2 RightBottomStart, Vector2 leftBottomStart, 
        Vector2 leftUpFinish, Vector2 RightUpFinish, Vector2 RightBottomFinish, Vector2 leftBottomFinish)
    {
        float startTime = Time.time;
        float fracComplete = (Time.time - startTime) / journeyTime;
        while (fracComplete <= 1)
        {
            if (stopAnimate)
                break;
            fracComplete = (Time.time - startTime) / journeyTime;

            LeftUpCorner.transform.localPosition = Vector2.Lerp(leftUpStart,leftUpFinish,fracComplete);
            RighUpCorner.transform.localPosition = Vector2.Lerp(RightUpStart, RightUpFinish, fracComplete);
            RightBottomCorner.transform.localPosition = Vector2.Lerp(RightBottomStart, RightBottomFinish, fracComplete);
            LeftBottomCorner.transform.localPosition = Vector2.Lerp(leftBottomStart,leftBottomFinish,fracComplete);
            yield return null;
        }
    }



}
