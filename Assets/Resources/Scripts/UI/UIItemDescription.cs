using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemDescription : MonoBehaviour
{

    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text description;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.title.text = "";
        this.description.text = "";
    }
    public void SetDescription(string itemName, string description)
    {
        this.title.text = itemName;
        this.description.text = description;
    }
}
