using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICraftItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityTxt;
    private bool empty = true;

    public event Action<UICraftItem> OnItemLeftClicked, OnItemEnterHandler, OnItemExitHandler;

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }
    public void SetData(Sprite sprite, string name)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = name + "";
        empty = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
            OnItemLeftClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnItemEnterHandler?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnItemExitHandler?.Invoke(this);
    }
}
