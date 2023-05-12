using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MagneticZone : MonoBehaviour
{

    [SerializeField] private MeltingStation station;
    [SerializeField] private MeltingRecipeSO recipes;

    private List<ItemSO> recipeComponents; //����� ������ ���������, ���� �� ������ � ����� ������������
    private void Start()
    {
        recipeComponents = new List<ItemSO>();
        for (int i = 0; i < recipes.Recipes.Count; i++)
        {
            recipeComponents.Add(recipes.Recipes[i].CraftIn()[0]);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6)&&(!station.itemInserted))
        {
            //���� ����� ������, �� ����� �������������� ��� (�������, ����� ��������� �� ���������������� � ��������� �� ����������� ��� �������)
            if (recipeComponents.Find(i => i.Name == collision.gameObject.GetComponent<Item>().InventoryItem.Name) == null)
            {
                //Debug.Log("������� ���");
                return;
            }
            //�� ������ ������
            if(collision.GetComponent<Item>()) 
            {
                station.SendToMelting(collision);
            }
        }
    }
}
