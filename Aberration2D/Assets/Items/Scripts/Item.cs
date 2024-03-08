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

    public virtual void OnShoot(int stacks)
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
        PlayerStats.instance.fireRate += 0.3f;
        PlayerStats.instance.burstDelay = (PlayerStats.instance.fireRate / 3) / PlayerStats.instance.burstSize;
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
        PlayerStats.instance.magazineSize += 5;
        PlayerStats.instance.reloadTime += 1f;
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

public class AutoTrigger : Item
{
    public override string GetName()
    {
        return "Auto Trigger";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.fireRate -= 0.01f;
    }
}

public class RocketBoots : Item
{
    public override string GetName()
    {
        return "Rocket Boots";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.numJumps += 1;
    }
}

public class GrenadeLauncher : Item
{
    public override string GetName()
    {
        return "Grenade Launcher";
    }

    public override void OnShoot(int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 10f)
        {
            Player.instance.GetComponent<PlayerShoot>().ShootGrenade();
        }
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
        PlayerStats.instance.rollSpeed *= 0.4f;
    }
}

public class BionicFinger : Item
{
    public override string GetName()
    {
        return "Bionic Finger";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.fireRate *= 0.5f;
        PlayerStats.instance.damage = Mathf.RoundToInt(PlayerStats.instance.damage * 0.3f);
        PlayerStats.instance.magazineSize += 10;
        PlayerStats.instance.reloadTime += 1;
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
    NosferatuBlood,
    BionicFinger,
    AutoTrigger,
    RocketBoots,
    GrenadeLauncher
}