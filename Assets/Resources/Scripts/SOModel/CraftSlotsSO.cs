using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftSlotsSO : ScriptableObject
{
    [SerializeField] private int quantitySlots;
    [SerializeField] private List<CraftableItem> inventoryItems;
   
    public int GetQuantitySlots()
    {
        return quantitySlots;
    }
}


[Serializable]
public struct CraftableItem
{
    public RecipeSO itemRecipe;
    public static CraftableItem GetEmptyItem()
        => new CraftableItem
        {
            itemRecipe = null
        };
}