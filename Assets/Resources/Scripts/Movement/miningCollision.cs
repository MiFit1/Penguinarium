using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miningCollision : MonoBehaviour
{
    public List<GameObject> collisionObjects;
    private void Start()
    {
        collisionObjects = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionObjects.Remove(collision.gameObject);
    }
}
