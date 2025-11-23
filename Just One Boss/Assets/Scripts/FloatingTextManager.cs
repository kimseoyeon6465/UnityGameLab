using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager instance;

    public GameObject floatingTextPrefab;
    private List<GameObject> textList = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }
    public static GameObject ShowText(string text, Vector3 worldPos)
    {
        GameUI gameUI = FindObjectOfType<GameUI>();

        // combo <= 0 이면 텍스트 생성 안 함
        if (gameUI != null && gameUI.GetCombo() <= 0)
            return null;

        Vector3 offsetPos = worldPos + new Vector3(0, 1.5f, 0);//발판과 겹치지 않게 오프셋값 추가
        Vector3 screenPos = Camera.main.WorldToScreenPoint(offsetPos);

        var go = Instantiate(instance.floatingTextPrefab, instance.transform);
        go.GetComponent<RectTransform>().position = screenPos;  
        go.GetComponent<Text>().text = text;

        instance.textList.Add(go);

        return go;
    }

    public static void ClearAll()
    {
        foreach (var t in instance.textList)
        {
            if (t != null)
                Destroy(t);
        }
        instance.textList.Clear();
    }
}
