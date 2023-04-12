using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryHotBar : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform SlotsRect;
    [SerializeField] private int HotBarSize = 8;
    [SerializeField] private InventorySO inventoryData;

    List<UIInventoryItem> listHotBarItems = new List<UIInventoryItem>();
    private void Start()
    {
        InitializeHotBarUI(HotBarSize);
        UpdateHotBar();
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
    }
    public void UpdateHotBar()
    {
        for(int i = 0; i < HotBarSize; i++)
        {
            InventoryItem invetoryItem = inventoryData.GetItemAt(i);
            if (!invetoryItem.IsEmpty)
            {
                listHotBarItems[i].SetData(invetoryItem.item.ItemImage, invetoryItem.quantity);
            }
        }
    }


}
