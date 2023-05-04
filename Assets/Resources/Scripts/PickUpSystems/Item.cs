using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; set; }

    [field: SerializeField]
    public int Quantity { get;  set; } = 1;


    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float journeyTime = 0.2f;
    [SerializeField] private float range = 1.02f;
    private bool stopAnimate = false;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        gameObject.transform.localScale = new Vector3(0.7f,0.7f,0.7f); //Убрать костыль (объкты создаются большего размера)
    }

    internal void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.layer != 6)
            stopAnimate = true;
    }   

    public void DumpItem(Vector3 position, Vector3 mousePosition )
    {
        gameObject.layer = 7;
        //GetComponent<Collider2D>().isTrigger = false;
        transform.position = position;
        StartCoroutine(AnimateItemDump(position, mousePosition));
    }
    private IEnumerator AnimateItemDump(Vector3 position, Vector3 mousePosition)
    {
        mousePosition.z = 0;
        Vector3 direction = mousePosition - position;
        //Debug.Log($"position = {mousePosition}");
        direction = direction.normalized;
        //Debug.Log($"directrion = {direction}");
        direction *= range;
        Vector3 hitcurrent = position + direction;
        float startTime = Time.time;
        float fracComplete = (Time.time - startTime) / journeyTime;
        while(fracComplete <= 1)
        {
            if (stopAnimate)
                break;
            fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Lerp(position, hitcurrent, fracComplete);
            yield return null;
        }
        
        gameObject.layer = 6;
        //GetComponent<Collider2D>().isTrigger = true;
    }
    
    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
