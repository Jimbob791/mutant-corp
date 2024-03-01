using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string GetName();

    public virtual void Update(GameObject player, int stacks)
    {

    }

    public virtual void OnShoot(GameObject player, int stacks)
    {

    }

    public virtual void OnPickup(GameObject player, int stacks)
    {

    }
}

public class Loudener : Item
{
    public override string GetName()
    {
        return "Loudener";
    }

    public override void OnPickup(GameObject player, int stacks)
    {
        player.GetComponent<PlayerShoot>().damage += Mathf.RoundToInt(player.GetComponent<PlayerStats>().damage * 0.1f);
        player.GetComponent<PlayerShoot>().bloomAngle += 0.25f;
    }
}

public class FleshBoots : Item
{
    public override string GetName()
    {
        return "Boots of Flesh";
    }

    public override void OnPickup(GameObject player, int stacks)
    {
        player.GetComponent<PlayerMove>().moveSpeed += 0.1f;
        player.GetComponent<PlayerMove>().jumpForce -= 0.1f;
    }
}

public class MutantBullets : Item
{
    public override string GetName()
    {
        return "Mutant Bullets";
    }

    public override void OnPickup(GameObject player, int stacks)
    {
        player.GetComponent<PlayerShoot>().damage += Mathf.RoundToInt(player.GetComponent<PlayerStats>().damage * 0.5f);
        player.GetComponent<PlayerShoot>().magazineSize -= 3;
    }
}