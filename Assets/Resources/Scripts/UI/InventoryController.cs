using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private int InventorySize;

    private void Start()
    {
        inventoryUI.InitializeInventoryUI(InventorySize);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
                inventoryUI.Show();
            else
                inventoryUI.Hide();
        }
    }
}