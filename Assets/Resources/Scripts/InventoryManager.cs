using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject invObject;
    public InventorySystem inv;
    public GameObject uiinv;

    public void Start()
    {
        uiinv = transform.GetChild(0).gameObject;
        invObject = GameObject.Find("InventorySystem");
        inv = invObject.GetComponent<InventorySystem>();
        UpdateInventory();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            uiinv.SetActive(!uiinv.activeSelf);
        }
    }

    public void UpdateInventory()
    {
        InventorySystem.InventoryItem item;
        for (int i = 0; i < (uiinv.transform.childCount - 1); i++)
        {
            if (inv.inventory.Count <= i || inv.inventory.Count == 0)
            {
                uiinv.transform.GetChild(i + 1).GetComponent<ItemSlot>().Set(null);
            }
            else
            {
                item = inv.inventory[i];
                uiinv.transform.GetChild(i + 1).GetComponent<ItemSlot>().Set(item);
            }
        }
    }

    

}
