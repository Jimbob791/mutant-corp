using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] List<string> stages = new List<string>();
    public float difficulty = 0;
    public string mode;
    [SerializeField] float globalTimer;
    [SerializeField] GameObject slamSFX;    
    [SerializeField] GameObject closeSFX;   
    [SerializeField] GameObject openSFX;   

    bool count = true;

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
            count = true;
            Time.timeScale = 1;
            globalTimer = 0;
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
            if (mode == "EasyTime" && difficulty == 5)
            {
                count = false;
                PlayerPrefs.SetFloat("EasyTime", globalTimer);
                GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
                StartCoroutine(Load("Menu"));
                return;
            }
            if (mode == "MedTime" && difficulty == 8)
            {
                count = false;
                PlayerPrefs.SetFloat("MedTime", globalTimer);
                GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
                StartCoroutine(Load("Menu"));
                return;
            }
            if (mode == "HardTime" && difficulty == 15)
            {
                count = false;
                PlayerPrefs.SetFloat("HardTime", globalTimer);
                GameObject.Find("Loading").GetComponent<Animator>().SetBool("load", true);
                StartCoroutine(Load("Menu"));
                return;
            }
        }
        else if (mode == "HardTime")
        {
            difficulty = 4;
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

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "Mutations" && SceneManager.GetActiveScene().name != "Tutorial")
        {
            if (count) globalTimer += Time.deltaTime;
            GameObject.Find("TimerDisplay").GetComponent<TextMeshProUGUI>().text = ConvertTimeToString(globalTimer);
            GameObject.Find("TimerDisplayBack").GetComponent<TextMeshProUGUI>().text = ConvertTimeToString(globalTimer);
        }
    }

    private string ConvertTimeToString(float time)
    {
        float hours = Mathf.FloorToInt(time / 3600);
        float minutes = Mathf.FloorToInt((time / 60) % 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliSeconds = Mathf.FloorToInt((time % 1) * 1000);
 
        return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliSeconds.ToString("00");
    }
}