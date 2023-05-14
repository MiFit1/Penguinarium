using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private AudioSource audioSource;

    public List<int> pickedUpItems; //(�������) ������ ������� ����������� ���������
    private void Start()
    {
        pickedUpItems = new List<int>();
    }

    private IEnumerator RemoveAfterTime(int ID)
    {
        yield return new WaitForSeconds(2f);
        pickedUpItems.Remove(ID);
    }
    private bool Find(int ID)
    {
        for (int i = 0; i < pickedUpItems.Count; i++)
        {
            if (pickedUpItems[i] == ID)
                return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            //Debug.Log(item.Quantity);
            //Debug.Log(item.GetInstanceID());
            if (!Find(item.GetInstanceID())) //(�������) ��������� ��������� 2 ����, ���� � �������� ����� �� ����������� ���������
            {
                pickedUpItems.Add(item.GetInstanceID()); //���������� � ������ ������� ����������� ���������
                StartCoroutine(RemoveAfterTime(item.GetInstanceID())); //�������� �� ������ ����� �����
                AudioClip clip = item.InventoryItem.AudioClip;
                audioSource.clip = clip;
                audioSource.Play();
                int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                if (reminder == 0)
                {
                    item.GetComponent<Collider2D>().enabled = false;
                    //Debug.Log(item.GetComponent<Collider2D>().enabled);
                    item.DestroyItem();
                }
                else
                    item.Quantity = reminder;
            }
        }
    }
}
