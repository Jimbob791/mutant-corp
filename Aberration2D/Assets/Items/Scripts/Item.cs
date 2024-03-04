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

    public virtual void OnPickup(int stacks)
    {

    }

    public virtual void OnReload(int stacks)
    {

    }
}

// -------------------------------------------------------------- Items --------------------------------------------------------------

public class Loudener : Item
{
    public override string GetName()
    {
        return "Loudener";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.damage += 2;
        PlayerStats.instance.bloomAngle += 2;
    }
}

public class FleshBoots : Item
{
    public override string GetName()
    {
        return "Boots of Flesh";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.moveSpeed += 0.1f;
        PlayerStats.instance.jumpForce += 0.1f;
    }
}

public class MutantBullets : Item
{
    public override string GetName()
    {
        return "Mutant Bullets";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.damage += 4;
        PlayerStats.instance.magazineSize -= 2;
    }
}

public class BurstSwitch : Item
{
    public override string GetName()
    {
        return "Burst Switch";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.magazineSize += 2;
        PlayerStats.instance.burstSize += 2;
        PlayerStats.instance.fireRate += 0.6f;
        PlayerStats.instance.burstDelay = (PlayerStats.instance.fireRate / 2) / PlayerStats.instance.burstSize;
    }
}

public class AmmoCache : Item
{
    public override string GetName()
    {
        return "Ammo Cache";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.magazineSize += 10;
        PlayerStats.instance.reloadTime += 1.5f;
    }
}

public class SaltedOrgans : Item
{
    public override string GetName()
    {
        return "Salted Organs";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.maxHealth += 50;
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(-50, true);
    }
}

public class NosferatuBlood : Item
{
    public override string GetName()
    {
        return "Nosferatu Blood";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.lifeSteal += 1;
    }
}

// -------------------------------------------------------------- Mutations --------------------------------------------------------------

public class SpringReload : Item
{
    public override string GetName()
    {
        return "Spring-loaded Reload";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.reloadTime = PlayerStats.instance.reloadTime / 2;
    }

    public override void OnReload(int stacks)
    {
        Player.instance.GetComponent<PlayerMove>().Jump(1 + stacks / 4, true);
    }
}

public class DemonicBlood : Item
{
    public override string GetName()
    {
        return "Demonic Blood";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.selfDamage += 1;
        PlayerStats.instance.fireRate *= 2;
        PlayerStats.instance.damage = Mathf.RoundToInt(PlayerStats.instance.damage * 2f);
    }
}

public class AllSeeingBullets : Item
{
    public override string GetName()
    {
        return "All-seeing Bullets";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.homingStrength += 0.1f;
        PlayerStats.instance.damage = Mathf.RoundToInt(PlayerStats.instance.damage * 0.9f);
    }
}

public class SteelHeart : Item
{
    public override string GetName()
    {
        return "Steel Heart";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.moveSpeed *= 0.6f;
        PlayerStats.instance.jumpForce *= 0.9f;
        PlayerStats.instance.maxHealth *= 3;
    }
}

public class AcrobaticMuscle : Item
{
    public override string GetName()
    {
        return "Acrobatic Muscle";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.moveSpeed *= 1.5f;
        PlayerStats.instance.jumpForce *= 1.1f;
        PlayerStats.instance.rollSpeed *= 0.2f;
    }
}

public enum Items
{
    Loudener,
    FleshBoots,
    MutantBullets,
    SpringReload,
    DemonicBlood,
    AllSeeingBullets,
    SteelHeart,
    AcrobaticMuscle,
    AmmoCache,
    BurstSwitch,
    SaltedOrgans,
    NosferatuBlood
}