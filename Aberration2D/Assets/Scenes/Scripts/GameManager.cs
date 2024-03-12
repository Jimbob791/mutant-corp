using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] List<string> stages = new List<string>();
    public float difficulty = 0;
    [SerializeField] GameObject slamSFX;    
    [SerializeField] GameObject closeSFX;   
    [SerializeField] GameObject openSFX;   

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Mutations" && scene.name != "Menu")
        {
            ItemStorage.instance.LoadItems();
            
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].GetComponent<EnemySpawner>() != null) spawners[i].GetComponent<EnemySpawner>().difficultyMultiplier = Mathf.Pow(1.5f, (difficulty - 2));
                if (spawners[i].GetComponent<ObjectiveSpawner>() != null) spawners[i].GetComponent<ObjectiveSpawner>().difficultyMultiplier = Mathf.Pow(1.5f, (difficulty - 2));
            }

            PlayerStats.instance.SetPlayerStats();
        }
        else if (scene.name == "Mutations")
        {
            Time.timeScale = 1;
            difficulty += 1;
        }
        else if (scene.name == "Menu")
        {
            Time.timeScale = 1;
            BaseStats baseStats = GameObject.Find("StatStorage").GetComponent<BaseStats>();
            PlayerStats stats = PlayerStats.instance;
            stats.moveSpeed = baseStats.moveSpeed;
            stats.jumpForce = baseStats.jumpForce;
            stats.numJumps = baseStats.numJumps;

            stats.rollSpeed = baseStats.rollSpeed;

            stats.fireRate = baseStats.fireRate;
            stats.recoil = baseStats.recoil;
            stats.magazineSize = baseStats.magazineSize;
            stats.burstSize = baseStats.burstSize;
            stats.burstDelay = baseStats.burstDelay;
            stats.reloadTime = baseStats.reloadTime;
            stats.bloomAngle = baseStats.bloomAngle;
            stats.damagePerData = baseStats.damagePerData;
            stats.droneStacks = baseStats.droneStacks;

            stats.maxHealth = baseStats.maxHealth;
            stats.lifeSteal = baseStats.lifeSteal;
            stats.regen = baseStats.regen;
            stats.lives = baseStats.lives;

            stats.damage = baseStats.damage;
            stats.homingStrength = baseStats.homingStrength;
            stats.selfDamage = baseStats.selfDamage;
            stats.bulletSpeed = baseStats.bulletSpeed;
            stats.range = baseStats.range;

            ItemStorage items = GameObject.Find("StatStorage").GetComponent<ItemStorage>();
            items.items = new List<ItemList>();
            difficulty = 0;
        }

        StartCoroutine(OpenSound());
    }

    IEnumerator OpenSound()
    {
        Instantiate(slamSFX);
        yield return new WaitForSeconds(0.15f);
        Instantiate(openSFX);
    }

    public void LoadMutations()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            ItemStorage.instance.SaveItems(Player.instance.GetComponent<PlayerItems>().items);
        }
        GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
        StartCoroutine(Load("Mutations"));
    }

    public void LoadMenu()
    {
        GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
        StartCoroutine(Load("Menu"));
    }

    public void LoadTutorial()
    {
        GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
        StartCoroutine(Load("Tutorial"));
    }

    public void ExitMutations()
    {
        GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
        StartCoroutine(Load(stages[Random.Range(0, stages.Count)]));
    }

    IEnumerator Load(string sceneName)
    {
        Instantiate(closeSFX);
        yield return new WaitForSeconds(1f);
        Instantiate(slamSFX);
        yield return new WaitForSeconds(0.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    } 

    public void TimeFreeze(float t)
    {
        StartCoroutine(Freeze());
    }

    private IEnumerator Freeze()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.03f);
        Time.timeScale = 1;
    }

    public void Shake(float duration, float strength)
    {
        StartCoroutine(ShakeCamera(duration, strength));
    }

    private IEnumerator ShakeCamera(float duration, float strength)
    {
        float elapsed = 0;
        GameObject target = GameObject.Find("Follow");
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.transform.localPosition = new Vector3(Random.Range(-strength, strength), Random.Range(-strength, strength), 0);
            yield return null;
        }
        target.transform.localPosition = Vector3.zero;
    }
}