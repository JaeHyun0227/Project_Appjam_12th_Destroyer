using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레이저 자국
public class LazerTrail : MonoBehaviour {

    public float Duration;

    public SpriteRenderer sprRen;

    public void SetTrail(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        timer = 0;
    }

    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        Vector4 color = sprRen.color;
        color.w = Mathf.Lerp(1, 0, timer / Duration);
        sprRen.color = color;

        if (timer > Duration) gameObject.SetActive(false);
    }
}
