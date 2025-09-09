using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts; // 인스펙터에 하트 오브젝트들 드래그
    [SerializeField] private GameObject[] emptyHearts; // 인스펙터에 하트 오브젝트들 드래그
    private int currentIndex;

    private void Start()
    {
        currentIndex = hearts.Length - 1; // 마지막 하트부터 제거
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
            Debug.Log("Game Over!");
            // TODO: 게임 오버 처리
        }
    }
}
