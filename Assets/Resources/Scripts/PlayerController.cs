using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private float vertical;

    private float maxSpeed = 8;
    float movingSpeed;

    private GameObject hand;

    private Vector3 mousePosition;
    private Vector3 diffHand;
    private float rotZHand;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        hand = this.transform.GetChild(0).GetChild(0).gameObject;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 && vertical != 0)
        {
            movingSpeed = 0.7f * maxSpeed;
        }
        else
        {
            movingSpeed = maxSpeed;
        }

        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * movingSpeed, vertical * movingSpeed);

        {
            diffHand = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hand.transform.position;
            diffHand.Normalize();
            rotZHand = Mathf.Atan2(diffHand.y, diffHand.x) * Mathf.Rad2Deg;

            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
            {
                if (rotZHand < -40)
                {
                    rotZHand = -40;
                }
                else if (rotZHand > 40)
                {
                    rotZHand = 40;
                }

                hand.transform.localRotation = Quaternion.Euler(0f, 0f, rotZHand);

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
            else
            {
                if (rotZHand > -140 && rotZHand < 0)
                {
                    rotZHand = -140;
                }
                else if (rotZHand < 140 && rotZHand > 0)
                {
                    rotZHand = 140;
                }

                hand.transform.localRotation = Quaternion.Euler(180f, 180f, -rotZHand);

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }

        } //вращение рукой
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<ItemObject>(out ItemObject item))
        {
            item.OnHandlePickupItem();
        }   
    }

}
