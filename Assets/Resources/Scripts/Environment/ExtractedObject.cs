using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class ExtractedObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float initialHP = 100f;
    [SerializeField] private ItemSO minedItem;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int NumberOfItems = 3;
    [SerializeField] private float clickDistance = 3f;
    [SerializeField] private Framing framing;
    [SerializeField] public AudioClip soundOfMining;
    [SerializeField] private miningCollision collision;
    [SerializeField] private ParticleSystem miningParticles;

    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 0.5f;
    private Vector3 initialPosition;

    private float currentHP;
    private bool mouseOnObject = false;
    private bool objectIsAvailable = false;
    private PlayerController player;

    public void SpawnMiningParticles()
    {
        miningParticles.Play();
    }
    private void SpawnObjects(int numberItems)
    {
        GameObject dropItem = itemPrefab;
        Item item = dropItem.GetComponent<Item>();
        item.InventoryItem = minedItem;
        item.Quantity = 1;
        for (int i = 0; i < numberItems; i++) 
        {
            GameObject spawnItem = Instantiate(dropItem);
            spawnItem.transform.position = transform.position;
            Rigidbody2D rb = spawnItem.GetComponent<Rigidbody2D>();
            rb.velocity = Random.insideUnitCircle.normalized * 2f;
        }
    }
    public void dealDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            SpawnObjects(NumberOfItems);
            //RemoveFromCollisionObjects();
            Destroy(gameObject);
        }
    }
    private void RemoveFromCollisionObjects()
    {
        for (int i = 0;i < collision.collisionObjects.Count; i++)
        {
            if (collision.collisionObjects[i] == gameObject)
            {
                collision.collisionObjects.RemoveAt(i);
                break;
            }
        }
    }
    public bool GetMouseOnObject()
    {
        return mouseOnObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOnObject = true;
        StartCoroutine(CheckDistanceToPlayer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnObject = false;
        objectIsAvailable = false;
        framing.StopAnimation();
    }
    
    private IEnumerator CheckDistanceToPlayer()
    {
        float distance;
        while (mouseOnObject)
        {
            distance = new Vector2(player.transform.position.x - transform.position.x,
                player.transform.position.y - transform.position.y).magnitude;
            if(distance <= clickDistance)
            {
                if(!objectIsAvailable)
                {
                    objectIsAvailable = true;
                    framing.StartAnimation();
                }
            }
            else
            {
                if(objectIsAvailable)
                {
                    objectIsAvailable = false;
                    framing.StopAnimation();
                }
            }
            yield return null;
        }
    }
    
    private void Start()
    {
        currentHP = initialHP;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public void StartShake()
    {
        initialPosition = transform.position;
        StartCoroutine(DoShake());
    }
    private IEnumerator DoShake()
    {
        float timer = 0;
        while (timer <= shakeDuration)
        {
            // вычисляем случайные значения для тряски объекта
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            transform.position = initialPosition + new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition; // возвращаем объект на начальную позицию
    }
}
