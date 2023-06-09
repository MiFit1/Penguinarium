using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    [field: SerializeField]  public bool IsStackable { get; set; }

    public int ID => GetInstanceID();

    [field:SerializeField] public float MeltingTime = 4f;
    [field: SerializeField] public int MazStackSize { get; set; } = 1;

    [field: SerializeField] public string Name { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string Description { get; set; }

    [field: SerializeField] public Sprite ItemImage { get; set; }

    [field: SerializeField] public AudioClip AudioClip { get; set; }
}
