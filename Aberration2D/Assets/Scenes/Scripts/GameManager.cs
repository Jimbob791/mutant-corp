using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        if (scene.name != "Mutations")
        {
            ItemStorage.instance.LoadItems();
            PlayerStats.instance.SetPlayerStats();
        }
    }

    public void LoadMutations()
    {
        ItemStorage.instance.SaveItems(Player.instance.GetComponent<PlayerItems>().items);
        StartCoroutine(Load("Mutations"));
    }

    public void ExitMutations()
    {
        StartCoroutine(Load("TestScene"));
    }

    IEnumerator Load(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    } 
}