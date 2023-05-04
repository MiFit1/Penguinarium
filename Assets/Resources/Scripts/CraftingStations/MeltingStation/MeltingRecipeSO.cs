using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MeltingRecipeSO : ScriptableObject
{
    [SerializeField] public List<RecipeSO> Recipes; 
}
