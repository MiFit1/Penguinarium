using System;
using System.Collections.Generic;
using UnityEngine;


public class InventoryHotBar : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform SlotsRect;
    [SerializeField] private InventorySO inventoryData;

    private int SelectedSlot = 0;

    List<UIInventoryItem> listHotBarItems = new List<UIInventoryItem>();

    private void Start()
    {
        PlayerController.ClickOnNumber += OnNumberClicked;
        PlayerController.OnScrolledRight += OnScrolledRight;
        PlayerController.OnScrolledLeft += OnScrolledLeft;
        listHotBarItems[0].Select();
    }
    public int GetHotBarSelectedSlot()
    {
        return SelectedSlot;
    }
    private void OnScrolledLeft()
    {
        if(SelectedSlot > 0)
        {
            DeselectAllItems();
            SelectedSlot --;
            listHotBarItems[SelectedSlot].Select();
        }
        else
        {
            DeselectAllItems();
            SelectedSlot = (listHotBarItems.Count - 1);
            listHotBarItems[SelectedSlot].Select();
        }
    }

    private void OnScrolledRight()
    {
        DeselectAllItems();
        SelectedSlot = (SelectedSlot + 1) % listHotBarItems.Count;
        listHotBarItems[SelectedSlot].Select();
    }

    private void OnNumberClicked(int itemIndex)
    {
        itemIndex--;
        if(itemIndex > listHotBarItems.Count)
        {
            return;
        }
        DeselectAllItems();
        SelectedSlot = itemIndex;
        listHotBarItems[itemIndex].Select();
    }
     private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listHotBarItems)
        {
            item.Deselect();
        }
    }
    public void InitializeHotBarUI(int hotBarSize)
    {
        for (int i = 0; i < hotBarSize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(SlotsRect);
            uiItem.transform.localScale = Vector3.one;
            listHotBarItems.Add(uiItem);
        }
       // Debug.Log($"HotBarlist = {listHotBarItems.Count}");
    }
    public void UpdateHotBar(int hotBarSize)
    {
        for(int i = 0; i < hotBarSize; i++)
        {
            InventoryItem invetoryItem = inventoryData.GetItemAt(i);
            if (!invetoryItem.IsEmpty)
            {
                listHotBarItems[i].SetData(invetoryItem.item.ItemImage, invetoryItem.quantity);
               // Debug.Log($"list = {listHotBarItems.Count}");
            }
            else
            {
                listHotBarItems[i].ResetData();
            }
        }
    }
   


}
