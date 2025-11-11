using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()//Q:이게 무슨 말임? instacne가 뭐임
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
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
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;

    //Logic
    public int pesos;
    public int experience;
    private bool hasLoaded = false; // 전역 변수 추가


    //Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        //is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            SaveState();
            PlayerPrefs.Save(); // 반드시!

            Debug.Log("[TryUpgradeWeapon] SaveState called.");

            return true;
        }

        return false;
    }

    
    //Experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count)//Max level
                return r;
        }
        return r;

    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r < level)
        {
            xp+= xpTable[r];
            r++;
        }
        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        Debug.Log("Level up!");
        player.OnLevelUp();
    }

    //Save state
    public void SaveState()
    {
        if (weapon == null)
            weapon = FindObjectOfType<Weapon>();
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
        PlayerPrefs.Save(); // 꼭 추가!

        //Debug.Log($"[SaveState] Saved: {s}");
    }

    public void LoadState(UnityEngine.SceneManagement.Scene s, LoadSceneMode mode)
    {
        //Debug.Log("SaveState Raw: " + PlayerPrefs.GetString("SaveState"));

        //if (hasLoaded) return; // 이미 한 번 불렀으면 스킵
        //hasLoaded = true;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //Change player skin
        pesos = int.Parse(data[1]);
        //Experience
        experience = int.Parse(data[2]);
        if(GetCurrentLevel()!=1)
            player.SetLevel(GetCurrentLevel()); 

        //Change the weapon Level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        //instance.StartCoroutine(SetPlayerSpawnAfterFrame());
    }

    
}


