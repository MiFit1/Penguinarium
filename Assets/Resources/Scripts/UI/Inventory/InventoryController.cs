using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private int HotBarSize = 7;
    [SerializeField] private InventoryHotBar invenoryHotBar;
    [SerializeField] private Item defaultItemPrefab;
    [SerializeField] private PlayerController player;
    

    public List<InventoryItem> initialItems = new List<InventoryItem>();

    private void Start()
    {
        PrepareUI();
        invenoryHotBar.InitializeHotBarUI(HotBarSize);
        PrepareInventory();
        invenoryHotBar.UpdateHotBar(HotBarSize);
        //inventoryUI.Show();
    }

    private void PrepareInventory()
    {
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        PlayerController.OnDropedItem += DropItem;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            inventoryData.AddItem(item);
        }
    }

    private int IndexDropedItem()
    {
        int selectedSlotHotBar = invenoryHotBar.GetHotBarSelectedSlot();
        int selectedSlotInventory = inventoryUI.GetSelectedSlotInventory();
        if (!inventoryUI.isActiveAndEnabled)
            return selectedSlotHotBar;
        else if (selectedSlotInventory == -1)
            return selectedSlotHotBar;
        else
            return selectedSlotInventory;
    }
    private void DropItem(int amount)
    {
        int dropItemIndex = IndexDropedItem();
        if (inventoryData.GetItemAt(dropItemIndex).IsEmpty)
            return;
        Item dropItem = Instantiate(defaultItemPrefab, Vector3.one,Quaternion.identity);
        dropItem.InventoryItem = inventoryData.GetItemAt(dropItemIndex).item;
        if (amount == -1)
        {
            dropItem.Quantity = inventoryData.GetItemAt(dropItemIndex).quantity;
        }
        else
        {
            dropItem.Quantity = amount;
        }
        Vector3 PlayerPosition = player.GetPlayerPosition();
       // Debug.Log($"{PlayerPosition}");
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log($"{MousePosition}");
        dropItem.DumpItem(PlayerPosition, MousePosition);
        inventoryUI.DropedSelectedItem(amount, dropItemIndex);
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
        invenoryHotBar.UpdateHotBar(HotBarSize);
    }

    private void PrepareUI()
    {
        //Debug.Log($"inventoryData.Size = {inventoryData.Size}");
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }
            
        ItemSO item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.name, item.Description);
    }

    private void HandleItemActionRequest(int itemIndex)
    {

    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            return;
        }
        inventoryUI.CreateDreggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        inventoryData.SwapItems(itemIndex_1, itemIndex_2);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key,
                        item.Value.item.ItemImage,
                        item.Value.quantity);
                }
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}
