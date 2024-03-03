using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Mutation
{
    public abstract string GetName();
    public abstract string GetDescription();

    public virtual void OnSelect(GameObject playerMutations)
    {
        
    }
}

//"Fast-twitch muscle fibers on your gun. Halves reload time. Automatically jumps when reloading.";