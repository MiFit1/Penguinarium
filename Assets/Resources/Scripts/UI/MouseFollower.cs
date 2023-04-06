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
        canvas = transform.root.GetComponent<Canvas>(); //берёт компонент Canvas
        mainCam = Camera.main;
        item = GetComponentInChildren<UIInventoryItem>(); // находит первый объект с таким компоненто в иерархии
    }

    public void SetData(Sprite sprite, int quantity) // устанавливает данные для объкта, следующего за курсором
    {
        item.SetData(sprite, quantity);
    }

    public void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
            Input.mousePosition, canvas.worldCamera, out position); //Устанавливает координаты position относительно UI
        transform.position = canvas.transform.TransformPoint(position);
    }

}
