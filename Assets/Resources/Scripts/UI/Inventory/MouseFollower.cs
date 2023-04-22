using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private UIInventoryItem item;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); 
        item = GetComponentInChildren<UIInventoryItem>(); // находит первый объект с таким компонентом в иерархии
    }

    public void SetData(Sprite sprite, int quantity) 
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

    public void Toggle(bool val)
    {
        //Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
}
