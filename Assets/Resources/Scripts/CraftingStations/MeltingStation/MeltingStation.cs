using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeltingStation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float ClickDistance = 1;
    [SerializeField] private PopupHint popupHint;
    [SerializeField] private int quantityMeltingThings = 5;
    [SerializeField] private InventoryHotBar hotBar;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private ItemSO LensSO;
    [SerializeField] private GameObject StationImageObj;
    [SerializeField] private Sprite spriteWithLens;
    [SerializeField] private Sprite spriteWithoutLens;
    [SerializeField] private MeltingRecipeSO recipes;
    [SerializeField] private float meltingTime = 10f;
    [SerializeField] private GameObject prefabOutItem;

    [SerializeField] private GameObject finishPosition;
    [SerializeField] private float journeyTime = 0.2f;
    [SerializeField] private float yCorrection = -0.1f;

    private bool OnStationHandler = false;
    private bool interfaceIsActive = false;
    private Rigidbody2D playerRB;
    private int CurrentQuantityMeltingThings;
    private bool lensInserted = false;
    private SpriteRenderer spriteStation;

    public bool itemInserted = false;

    /*    private float distanceToStation(PlayerController controller)
        {
            Vector2 distance = controller.transform.position - transform.position;
            return distance.magnitude;
        }*/
    public void SendToMelting(Collider2D collision)
    {
        itemInserted = true;
        Vector3 center = new Vector3((collision.gameObject.transform.position.x + finishPosition.transform.position.x) / 2f,
                (collision.gameObject.transform.position.y + finishPosition.transform.position.y) / 2f, 0);

        Vector3 finishVector = finishPosition.transform.position - center;
        finishVector.y += yCorrection;
        Vector3 startVector = collision.gameObject.transform.position - center;
        collision.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(FlightToMeltingStation(startVector, finishVector, center, collision));
    }

    private int FindNumberRecipe(Collider2D collision)
    {
        for (int i = 0; i < recipes.Recipes.Count; i++)
        {
            if(collision.gameObject.GetComponent<Item>().InventoryItem == recipes.Recipes[i].CraftIn()[0])
            {
                return i;
            }
        }
        return -1;
    }
    private IEnumerator FlightToMeltingStation(Vector3 StartVector, Vector3 FinishVector, Vector3 Center, Collider2D collision)
    {
        float startTime = Time.time;
        float fracComplete = (Time.time - startTime) / journeyTime;
        while (fracComplete <= 1)
        {
            fracComplete = (Time.time - startTime) / journeyTime;
            collision.gameObject.transform.position = Vector3.Slerp(StartVector, FinishVector, fracComplete) + Center;
            yield return null;
        }
        while (lensInserted == false)
            yield return null;
        StartCoroutine(StartMelting(FindNumberRecipe(collision), collision)); //вызывается тут, потому что код после корутины выполняется сразу, а не по её завершении
        
    }
    public IEnumerator StartMelting(int IndexRecipe, Collider2D collision)
    {
        float startTime = Time.time;
        while ((Time.time - startTime) <= meltingTime)
            yield return null;
        Destroy(collision.gameObject);
        CurrentQuantityMeltingThings--;
        itemInserted = false;
        if (CurrentQuantityMeltingThings <= 0)
        {
            lensInserted = false;
            spriteStation.sprite = spriteWithoutLens;//анимацию ломания линзы сюда
        }
        GameObject outItem = prefabOutItem;
        outItem.GetComponent<Item>().InventoryItem = recipes.Recipes[IndexRecipe].CraftOut();
        outItem.GetComponent<Item>().Quantity = 1;
        GameObject meltedItem= Instantiate(outItem);
        meltedItem.transform.position = finishPosition.transform.position;
        meltedItem.GetComponent<Rigidbody2D>().velocity = new Vector2(5, -1);

    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private void Start()
    {
        playerRB = playerController.GetComponent<Rigidbody2D>();
        PlayerController.OnInteraction += OnInteractionButton;
        CurrentQuantityMeltingThings = quantityMeltingThings;
        spriteStation = StationImageObj.GetComponent<SpriteRenderer>();
        spriteStation.sprite = spriteWithoutLens;
    }

    private void OnInteractionButton()
    {
       if((interfaceIsActive)&&(!lensInserted))
        {
            int itemIndex = hotBar.GetHotBarSelectedSlot();
            InventoryItem item = inventory.GetItemAt(itemIndex);
            if (LensSO.Name != item.item.Name)
                return;
            inventory.RemoveAllItems(itemIndex);
            lensInserted = true;
            CurrentQuantityMeltingThings = quantityMeltingThings;
            spriteStation.sprite = spriteWithLens;

        }
    }

    public void ActivateInterface()
    {
        if (OnStationHandler == true)
        {
            if (interfaceIsActive == false)
            {
                popupHint.Show();
                interfaceIsActive = true;
                popupHint.StartAnimation();
            }
        }
    }
    public void DeactivateInterface()
    {
        interfaceIsActive = false;
        popupHint.StopAnimation();
        popupHint.Hide();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       OnStationHandler = true;
        playerRB.WakeUp(); //из-за оптимизации Unity, если объект не движется, то RB не работает
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnStationHandler = false;
        DeactivateInterface();
    }

}


//старый код
/*using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeltingStation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UICraftPage craftPage;
    [SerializeField] private ClickDistance distanceCollider;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int DistanceClick;

    private void Start()
    {
        distanceCollider.OnTriggerExit += TriggerExit;
    }

    private void TriggerExit()
    {

        if (craftPage.isActiveAndEnabled == true)
        {
            craftPage.gameObject.SetActive(false);
            craftPage.SetCurrentStationID(-1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPressRaycast.gameObject.layer == 2) //костыли (срабатывает на нажатие на ненужный коллайдер)
            return;
        if (eventData.pointerId == -2)
        {
            Vector3 distance = playerController.transform.position - distanceCollider.transform.position;
            distance.z = 0;
            if (distance.magnitude <= DistanceClick)
            {
                if (craftPage.isActiveAndEnabled == false)
                {
                    craftPage.gameObject.SetActive(true);
                    craftPage.SetCurrentStationID(this.GetInstanceID());
                }
                else
                {
                    craftPage.gameObject.SetActive(false);
                    craftPage.SetCurrentStationID(-1);
                }
            }
        }
    }
}
*/