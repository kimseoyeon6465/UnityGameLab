using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts; // 인스펙터에 하트 오브젝트들 드래그
    [SerializeField] private GameObject[] emptyHearts; // 인스펙터에 하트 오브젝트들 드래그
    private int currentIndex;


    [Header("Combo")]
    public Text comboText;
    private int combo = 0;

    [Header("Score")]
    public Text scoreText;
    private int score = 0;

    [Header("Rage")]
    public Slider rageSlider;
    private int rage = 0;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    private void Start()
    {
        currentIndex = hearts.Length - 1; // 마지막 하트부터 제거
    }

    public int GetScore() => score;
    public int GetRageValue() => rage;
    public int GetCombo() => combo;
    public void AddCombo()
    {
        combo++;
        comboText.text = combo + " Combo!";
        comboText.gameObject.SetActive(true);

    }
    public void ReduceHeart()
    {
        if (currentIndex < 0) return;

        Debug.Log($"하트 감소! 남은 하트: {currentIndex}");

        // 1) 비활성화 하려면 ↓
        hearts[currentIndex].SetActive(false);
        emptyHearts[currentIndex].SetActive(true);
        currentIndex--;

        if (currentIndex < 0)
        {
            GameOver();
            // TODO: 게임 오버 처리
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;   // 게임 멈춤
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void AddRage(int amount)
    {
        rage += amount;
        rageSlider.value = rage;
    }

    public void ResetCombo()
    {
        combo = 0;
        comboText.gameObject.SetActive(false);
    }
}
