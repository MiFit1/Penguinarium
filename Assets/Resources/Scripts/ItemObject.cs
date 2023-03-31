using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;
    public GameObject invObject;
    public InventorySystem inv;

    private SpriteRenderer render;

    private void Start()
    {
        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        invObject = GameObject.Find("InventorySystem");
        inv = invObject.GetComponent<InventorySystem>();

        render.sprite = referenceItem.icon;
    }

    public void OnHandlePickupItem()
    {
        inv.Add(referenceItem);
        Destroy(this.gameObject);
    }
}
