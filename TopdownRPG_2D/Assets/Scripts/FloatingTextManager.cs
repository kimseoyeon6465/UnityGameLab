using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();//Q:¿¨ ÀÌ·±°Íµµ µÊ?

    public void Show(string msg, int fontSize, Color color, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();
        floatingText.txt.text = msg;
        //floatingText.txt.fontSizeÇÒ Â÷·Ê
    }
    private FloatingText GetFloatingText()//Q:???¹«½¼ ±â´ÉÀÎÁö ÀÌÇØ ¸øÇÔ
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);
        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();
        }
        return txt;

    }
}
