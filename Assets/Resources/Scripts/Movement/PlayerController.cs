using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private float vertical;

    public static Action<int> ClickOnNumber;
    public static Action OnScrolledRight, OnScrolledLeft;
    public static Action<int> OnDropedItem;
    public static Action OnInteraction;

    private PlayerInput input;

    private void Awake()
    {
        input = new PlayerInput();
        input.Player.ClickOnOne.performed += context => OnClickedOne();
        input.Player.ClickOnTwo.performed += context => OnClickedTwo();
        input.Player.ClickOnThree.performed += context => OnClickedThree();
        input.Player.ClickOnFour.performed += context => OnClickedFour();
        input.Player.ClickOnFive.performed += context => OnClickedFive();
        input.Player.ClickOnSix.performed += context => OnClickedSix();
        input.Player.ClickOnSeven.performed += context => OnClickedSeven();
        input.Player.Q.performed += context => OnQClicked();
        input.Player.QShift.performed += context => OnQShiftClicked();
        input.Player.E.performed += context => OnEClicked();
    }

    private void OnEClicked()
    {
        OnInteraction?.Invoke();
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
    private void OnQShiftClicked()
    {
        OnDropedItem?.Invoke(-1);
    }

    private void OnQClicked()
    {
        OnDropedItem?.Invoke(1);
    }

    private void OnClickedSeven()
    {
        ClickOnNumber?.Invoke(7);
    }
    private void OnClickedSix()
    {
        ClickOnNumber?.Invoke(6);
    }
    private void OnClickedFive()
    {
        ClickOnNumber?.Invoke(5);
    }
    private void OnClickedFour()
    {
        ClickOnNumber?.Invoke(4);
    }
    private void OnClickedThree()
    {
        ClickOnNumber?.Invoke(3);
    }
    private void OnClickedTwo()
    {
        ClickOnNumber?.Invoke(2);
       // Debug.Log("Clicked 2");
    }
    private void OnClickedOne()
    {
       // Debug.Log("Clicked 1");
        ClickOnNumber?.Invoke(1);
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    [SerializeField] private float maxSpeed;
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

        //Прокрутка хотбара
        float z = input.Player.Scroll.ReadValue<float>();
        if (z > 0)
        {
            OnScrolledRight?.Invoke();
        }
        else if(z < 0)
        {
            OnScrolledLeft?.Invoke();
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
}
