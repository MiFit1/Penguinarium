using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public int slotid;
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_quantity;

    public void Set(InventorySystem.InventoryItem item)
    {
        if (item != null)
        {
            m_icon.sprite = item.data.icon;
            m_quantity.text = item.stackSize.ToString();
        }
        else
        {
            m_icon.sprite = null;
            m_quantity.text = null;
        }
    }

}
