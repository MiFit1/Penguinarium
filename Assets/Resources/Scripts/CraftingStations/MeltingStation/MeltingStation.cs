using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeltingStation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PopupHint popupHint; 
    [SerializeField] private InventoryHotBar hotBar;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private ItemSO LensSO;
    [SerializeField] private GameObject StationImageObj;
    [SerializeField] private Sprite spriteWithLens;
    [SerializeField] private Sprite spriteWithoutLens;
    [SerializeField] private MeltingRecipeSO recipes; //сюда добавл€ть предметы, которые можно переплавить и то, что будет на выходе
    [SerializeField] private GameObject prefabOutItem;

    [SerializeField] private GameObject finishPosition; 
    [SerializeField] private float journeyTime = 0.2f;
    [SerializeField] private float yCorrection = -0.1f; //чтобы прит€гивать предметы чуть ниже центра, чтобы они нормально отрисовывались

    [SerializeField] private LensDamageBar lensBar;
    [SerializeField] private float lenseMeltingTime = 10f;

    [SerializeField] private GameObject mendingLight;
    [SerializeField] private ParticleSystem mendingParticles;

    private bool OnStationHandler = false;
    private bool interfaceIsActive = false;
    private Rigidbody2D playerRB;
    private bool lensInserted = false;
    private SpriteRenderer spriteStation;

    public bool itemInserted = false;

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
    
    //номер рецепта, если такой есть
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
    //јнимаци€ переноса предмета до места переплавки
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

        collision.GetComponent<Item>().animationIsStarted = false;
        
        while (true)
        {
            if(lensInserted == true)
            {
                //¬ключение / отключение частиц
                StartCoroutine(Particles());

                yield return new WaitForSeconds(collision.GetComponent<Item>().InventoryItem.MeltingTime);

                //если не хватило времени на переплавку, то запустит while сначала в следующем кадре
                if (lensInserted == false)
                {
                    yield return null;
                    continue;
                }
                  
                Destroy(collision.gameObject);
                itemInserted = false;

                //создание нового предмета;
                GameObject outItem = prefabOutItem;
                int IndexRecipe = FindNumberRecipe(collision);
                outItem.GetComponent<Item>().InventoryItem = recipes.Recipes[IndexRecipe].CraftOut();
                outItem.GetComponent<Item>().Quantity = 1;
                GameObject meltedItem = Instantiate(outItem);
                meltedItem.transform.position = finishPosition.transform.position;
                
                Vector2 flightDirection = playerController.transform.position - meltedItem.transform.position;
                flightDirection.Normalize();

                meltedItem.GetComponent<Rigidbody2D>().velocity = flightDirection * 6f;


                break;
            }
            yield return null;
        }
    }

    private IEnumerator Particles()
    {
        while (itemInserted)
        {
            if (lensInserted && mendingParticles.isStopped)
                mendingParticles.Play();
            if (!lensInserted && mendingParticles.isPlaying)
                mendingParticles.Stop();
            yield return null;
        }
        mendingParticles.Stop();
    }
    private void Start()
    {
        playerRB = playerController.GetComponent<Rigidbody2D>();
        PlayerController.OnInteraction += OnInteractionButton;
        spriteStation = StationImageObj.GetComponent<SpriteRenderer>();
        spriteStation.sprite = spriteWithoutLens;
        mendingLight.SetActive(false);
        mendingParticles.Stop();
    }
    //срабатывает при нажатии на кнопку E
    private void OnInteractionButton()
    {
       if((interfaceIsActive)&&(!lensInserted))
        {
            int itemIndex = hotBar.GetHotBarSelectedSlot();
            InventoryItem item = inventory.GetItemAt(itemIndex);
            if (LensSO.Name != item.item.Name) 
                return;
            inventory.RemoveItem(itemIndex, 1);
            lensInserted = true;
            lensBar.Show();
            mendingLight.SetActive(true);
            StartCoroutine(AnimationBar(lenseMeltingTime));
            spriteStation.sprite = spriteWithLens;
        }
    }
    
    //анимаци€ убывани€ бара
    public IEnumerator AnimationBar(float animationTime)
    {
        float startTime = Time.time;
        float fracComplete = 0f;
        float reverseFracComplete = 1f;
        while ((Time.time - startTime) <= animationTime)
        {
            fracComplete = (Time.time - startTime) / animationTime;
            reverseFracComplete = 1f - fracComplete;
            lensBar.SetFillLevel(reverseFracComplete);
            yield return null;
        }
        spriteStation.sprite = spriteWithoutLens;
        lensInserted = false;
        lensBar.Hide();
        mendingLight.SetActive(false);
        //анимацию ломани€ линзы сюда
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
        playerRB.WakeUp(); //из-за оптимизации Unity, если объект не движетс€, то RB не работает
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