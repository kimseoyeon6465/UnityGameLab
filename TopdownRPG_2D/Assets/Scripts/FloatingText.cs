using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText// 함수 오버라이드 안하고 just c#만 쓸거니까 Monobehaviour 상속안함
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active= true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
        {
            return;
        }
        if(Time.time-lastShown>duration)//일정 시간 지나면 사라지게
        {
            Debug.Log("Hide 호출");
            Hide();
        }
        go.transform.position += motion * Time.deltaTime;//떠다니게

    }
}
