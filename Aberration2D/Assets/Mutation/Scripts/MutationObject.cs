using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MutationObject", menuName = "ScriptableObjects/MutationObject")]
public class MutationObject : ScriptableObject
{
    public Items item;
    public Sprite icon;
    public string mutationName;
    [TextArea(3,5)] public string description;
}