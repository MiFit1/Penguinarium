using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensDamageBar : MonoBehaviour
{
    [SerializeField] GameObject Fill;
    private float maxfillScaleX;
    

    private void Start()
    {
        maxfillScaleX = Fill.transform.localScale.x;
        SetFillLevel(1f);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetFillLevel(float fillLevel)
    {
        Vector2 scaleVector = new Vector2(maxfillScaleX * fillLevel,Fill.transform.localScale.y);
        Fill.transform.localScale = scaleVector;
        
        float inverseFillLevel = 1f - fillLevel;
        Fill.transform.localPosition = new Vector2(1.6f * inverseFillLevel * (-1), Fill.transform.localPosition.y);
    }
}
