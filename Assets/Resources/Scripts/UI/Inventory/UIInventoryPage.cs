using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform slots;
    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private InventoryHotBar hotBar;
    List<UIInventoryItem> listUIItems = new List<UIInventoryItem>();


    private int currentlyDraggedItemIndex = -1;
    private int selectedSlotInventory = -1;


    public event Action<int> OnItemActionRequested, OnStartDragging, OnDescriptionRequested;
    public event Action<int, int> OnSwapItems;

    public int GetSelectedSlotInventory()
    {
        return selectedSlotInventory;
    }
    public void InitializeInventoryUI(int inventorysize)
    {
        for (int i = 0; i < inventorysize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(slots);
            uiItem.transform.localScale = Vector3.one;
            listUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    public void DropedSelectedItem(int amount, int itemIndex)
    {
        if (amount == -1)
        {
            inventoryData.RemoveAllItems(itemIndex);
             selectedSlotInventory = -1;
        }
        else
        {
            inventoryData.RemoveItem(itemIndex,amount);
            if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                selectedSlotInventory = -1;
            else if (selectedSlotInventory != -1)
                listUIItems[itemIndex].Select();
        }
    }
    private void Awake()
    {
        mouseFollower.Toggle(false);
        //Hide();
    }

    
    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listUIItems.Count > itemIndex)
        {
            listUIItems[itemIndex].SetData(itemImage,itemQuantity);
        }
           
    }
    private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
    {
        
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = listUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        HandleItemSelection(inventoryItemUI);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = listUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDreggedItem(Sprite sprite, int quantity)
    {
        //Debug.Log("toggle true");
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }
    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = listUIItems.IndexOf(inventoryItemUI);
        selectedSlotInventory = index;
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    public void ResetSelection()
    {
        //добавить сброс описания
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listUIItems)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        selectedSlotInventory = -1;
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

    internal void UpdateDescription(int itemIndex, string name, string description)
    {
        //itemDescription.SetDescription // доделать работу с описанием
        DeselectAllItems();
        listUIItems[itemIndex].Select();
        //Debug.Log("item selected");
    }

    internal void ResetAllItems()
    {
        foreach(var item in listUIItems)
        {
            item.ResetData();
            item.Deselect();
        }
    }
}
