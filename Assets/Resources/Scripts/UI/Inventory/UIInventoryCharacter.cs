using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryCharacter : MonoBehaviour
{
    private Animator anim;
    private GameObject hand;

    private Vector3 mousePosition;
    private Vector3 diffHand;
    private float rotZHand;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        hand = this.transform.GetChild(0).GetChild(0).gameObject;
    }

    void Update()
    {
        float horizontal;
        float vertical;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector2 vec = new Vector2 (horizontal, vertical);
        if (vec != Vector2.zero)
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
}
