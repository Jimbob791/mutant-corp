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

    public virtual void OnChest(int stacks)
    {

    }

    public virtual void OnHit(GameObject enemy, int stacks)
    {

    }

    public virtual void OnKill(GameObject enemy, int stacks)
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
        PlayerStats.instance.damage += 2;
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
        PlayerStats.instance.maxHealth += 20;
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(-20, true);
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
        PlayerStats.instance.lifeSteal += 2;
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

public class ThumperGL : Item
{
    public override string GetName()
    {
        return "Thumper GL";
    }

    public override void OnShoot(int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 10f)
        {
            Player.instance.GetComponent<PlayerShoot>().ShootGrenade();
        }
    }
}

public class Silencer : Item
{
    public override string GetName()
    {
        return "Silencer";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.bulletSpeed += 2;
    }
}

public class SniperScope : Item
{
    public override string GetName()
    {
        return "Sniper Scope";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.range += 0.2f;
    }
}

public class GreenMaple : Item
{
    public override string GetName()
    {
        return "Green Maple";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.regen += 1;
    }
}

public class JavelinSL : Item
{
    public override string GetName()
    {
        return "Javelin SL";
    }

    public override void OnHit(GameObject enemy, int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 5f)
        {
            Player.instance.GetComponent<PlayerShoot>().ShootJavelin();
        }
    }
}

public class Hourglass : Item
{
    public override string GetName()
    {
        return "Hourglass";
    }

    public override void OnReload(int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 5f)
        {
            Player.instance.GetComponent<PlayerShoot>().FinishReload();
        }
    }
}

public class RustedSickle : Item
{
    public override string GetName()
    {
        return "Rusted Sickle";
    }

    public override void OnHit(GameObject enemy, int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 15f)
        {
            enemy.GetComponent<EnemyHealth>().bleedDamage += stacks * 2;
        }
    }
}

public class PlasticC4 : Item
{
    public override string GetName()
    {
        return "Plastic C4";
    }

    public override void OnHit(GameObject enemy, int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 10f)
        {
            Player.instance.GetComponent<PlayerShoot>().SpawnC4(enemy, 5 * stacks + Mathf.RoundToInt(Player.instance.GetComponent<PlayerData>().data * Player.instance.GetComponent<PlayerShoot>().damagePerData));
        }
    }
}

public class GuardDogs : Item
{
    public override string GetName()
    {
        return "Guard Dogs";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.droneStacks += 1;
    }
}

public class TeslaShield : Item
{
    public override string GetName()
    {
        return "Tesla Shield";
    }

    public override void OnHit(GameObject enemy, int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 3f)
        {
            PlayerStats.instance.maxHealth += 1;
            Player.instance.GetComponent<PlayerHealth>().TakeDamage(-1, true);
        }
    }
}

public class Bacteriophage : Item
{
    public override string GetName()
    {
        return "Bacteriophage";
    }

    public override void OnHit(GameObject enemy, int stacks)
    {
        if (Random.Range(0f, 100f) < stacks * 5f)
        {
            enemy.GetComponent<EnemyHealth>().explode = true;
        }
    }
}

public class Dewdrop : Item
{
    public override string GetName()
    {
        return "Dewdrop";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.maxHealth += stacks * 5;
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(-stacks * 5, true);
    }
}

public class HealingCrystal : Item
{
    public override string GetName()
    {
        return "Healing Crystal";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.maxHealth += 50;
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(-50 * 5, true);
    }
}

public class SealOfApproval : Item
{
    public override string GetName()
    {
        return "Seal of Approval";
    }

    public override void OnChest(int stacks)
    {
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(-stacks * 5, true);
    }
}

public class Nectar : Item
{
    public override string GetName()
    {
        return "Nectar";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.moveSpeed += 0.25f;
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
        Player.instance.GetComponent<PlayerMove>().Jump(1 + stacks, true);
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
        PlayerStats.instance.homingStrength += 2f;
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
        PlayerStats.instance.reloadTime *= 1.8f;
        PlayerStats.instance.bulletSpeed *= 0.8f;
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
        PlayerStats.instance.moveSpeed *= 1.3f;
        PlayerStats.instance.jumpForce *= 1.05f;
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
        PlayerStats.instance.damage = Mathf.RoundToInt(PlayerStats.instance.damage * 0.5f);
        PlayerStats.instance.magazineSize += 10;
        PlayerStats.instance.reloadTime += 1;
    }
}

public class ShotgunCells : Item
{
    public override string GetName()
    {
        return "Shotgun Cells";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.fireRate *= 2;
        PlayerStats.instance.damage = Mathf.RoundToInt(PlayerStats.instance.damage * 0.6f);
        PlayerStats.instance.magazineSize += 20;
        PlayerStats.instance.burstSize += 6;
        PlayerStats.instance.burstDelay = 0;
        PlayerStats.instance.bloomAngle += 10;
        PlayerStats.instance.range -= 0.25f;
    }
}

public class DataLicense : Item
{
    public override string GetName()
    {
        return "Data License";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.damagePerData += 0.01f;
    }
}

public class PheonixTalon : Item
{
    public override string GetName()
    {
        return "Pheonix Talon";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.lives += 1;
    }
}

public class BipodHands : Item
{
    public override string GetName()
    {
        return "Bipod Hands";
    }

    public override void OnPickup(int stacks)
    {
        PlayerStats.instance.fireRate *= 4;
        PlayerStats.instance.damage = Mathf.RoundToInt(PlayerStats.instance.damage * 8);
        PlayerStats.instance.magazineSize = Mathf.RoundToInt(PlayerStats.instance.magazineSize / 4);
        PlayerStats.instance.bloomAngle = 0;
        PlayerStats.instance.range += 1f;
        PlayerStats.instance.bulletSpeed += 50f;
        PlayerStats.instance.reloadTime += 2;
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
    ThumperGL,
    SniperScope,
    Silencer,
    GreenMaple,
    ShotgunCells,
    JavelinSL,
    PheonixTalon,
    PlasticC4,
    RustedSickle,
    DataLicense,
    Hourglass,
    GuardDogs,
    Bacteriophage,
    TeslaShield,
    Dewdrop,
    HealingCrystal,
    BipodHands,
    Nectar,
    SealOfApproval
}