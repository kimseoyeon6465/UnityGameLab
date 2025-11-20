using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float moveSpeed = 40f;
    private Text text;

    private bool isAnimating = false;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (!isAnimating) return;
        transform.Translate(Vector3.up*moveSpeed*Time.deltaTime);

        Color c = text.color;
        c.a -= Time.deltaTime / fadeDuration;
        text.color = c;

        if (c.a <= 0f)
            Destroy(gameObject);
    }
    public void Play()
    {
        isAnimating = true;
    }
}
