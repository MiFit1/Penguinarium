/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDistance : MonoBehaviour
{
    [SerializeField] private UICraftPage craftPage;
    [SerializeField] private MeltingStation meltingStation;


    public event Action OnTriggerExit;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player Exit");
        if (craftPage.GetCurrentStationID() == meltingStation.GetInstanceID())
        {
            OnTriggerExit?.Invoke();
        }
    }
}
*/