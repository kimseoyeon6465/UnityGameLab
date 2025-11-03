using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()//Q:¿Ã∞‘ π´Ωº ∏ª¿”? instacne∞° ππ¿”
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    public Player player;
    //public weapon weapon..

    //Logic
    public int pesos;
    public int experiences;

    //Save state
    public void SaveState()
    {
        //string s = "";

        //s += "0" + "|";
        //s += pesos.ToString() + "|";
        //s += experiences.ToString() + "|";
        //s += "0";

        //PlayerPrefs.SetString("SaveState", s);
        Debug.Log("Save State");
    }

    public void LoadState(UnityEngine.SceneManagement.Scene s, LoadSceneMode mode) 
    {
        //if (!PlayerPrefs.HasKey("SaveState"))
        //{
        //    return;
        //}
        //string[] data =PlayerPrefs.GetString("SaveState").Split('|');

        ////Change player skin
        //pesos = int.Parse(data[1]);
        //experiences = int.Parse(data[2]);
        ////Change the weapon Level
        Debug.Log("LoadState");
    }
}


