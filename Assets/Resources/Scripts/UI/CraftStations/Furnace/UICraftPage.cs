using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class UICraftPage : MonoBehaviour
{
    [SerializeField] private CraftSlotsSO craftSlots;
    [SerializeField] private UICraftItem UICraftItemPrefab;
    [SerializeField] private RectTransform slots;
    private int CurrentStationID = -1;

    public int GetCurrentStationID()
    {
        return CurrentStationID;
    }
    public void SetCurrentStationID(int newID)
    {
         CurrentStationID = newID;
    }

    List<UICraftItem> listCraftItems = new List<UICraftItem>();

    private void Start()
    {
        InitializeCraftUI(craftSlots.GetQuantitySlots());
    }
    public void InitializeCraftUI(int quantitySlots)
    {
        for (int i = 0; i < quantitySlots; i++)
        {
            UICraftItem craftItem = Instantiate(UICraftItemPrefab, Vector3.zero, Quaternion.identity);
            craftItem.transform.SetParent(slots);
            craftItem.transform.localScale = Vector3.one;
            listCraftItems.Add(craftItem);

            craftItem.OnItemLeftClicked += LeftClicked;
            craftItem.OnItemEnterHandler += ItemEnter;
            craftItem.OnItemExitHandler += ItemExit;
        }
    }

    private void ItemExit(UICraftItem obj)
    {
        throw new NotImplementedException();
    }

    private void ItemEnter(UICraftItem obj)
    {
        throw new NotImplementedException();
    }

    private void LeftClicked(UICraftItem obj)
    {
        throw new NotImplementedException();
    }
}
