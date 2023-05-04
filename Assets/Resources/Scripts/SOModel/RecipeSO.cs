using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<ItemSO> craftIn;
    [SerializeField] private ItemSO craftOut;

    public List<ItemSO> CraftIn()
    {
        return craftIn;
    }
    public ItemSO CraftOut()
    {
        return craftOut;
    }
}

