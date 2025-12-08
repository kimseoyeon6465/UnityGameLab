using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TestPlayer
{
    // 런타임용
    public string name;
    public int score;
    private int combo;
}

class PlayerSaveData
{
    // 저장용
    public string name;
    public int score;
    private int combo;
}



public class TestPlayerData : MonoBehaviour
{
    TestPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = new TestPlayer();
        player.name = "TestPlayer";
        player.score = 0;
        
        
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.name = player.name;
        saveData.score = player.score;
        
        
        
        // 저장
        string playerData = JsonUtility.ToJson(saveData);
        Debug.Log(playerData);
        
        
        // 불러오기
        saveData = JsonUtility.FromJson<PlayerSaveData>(playerData);
        
        player.name = saveData.name;
        player.score = saveData.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
