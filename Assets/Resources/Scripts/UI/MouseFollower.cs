using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCam;
    [SerializeField] private UIInventoryItem item;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); //���� ��������� Canvas
        mainCam = Camera.main;
        item = GetComponentInChildren<UIInventoryItem>(); // ������� ������ ������ � ����� ���������� � ��������
    }

    public void SetData(Sprite sprite, int quantity) // ������������� ������ ��� ������, ���������� �� ��������
    {
        item.SetData(sprite, quantity);
    }

    public void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
            Input.mousePosition, canvas.worldCamera, out position); //������������� ���������� position ������������ UI
        transform.position = canvas.transform.TransformPoint(position);
    }

}
