using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    public float speed;

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.left * (speed + GameManager.GetInstance().plusSpeed)* Time.deltaTime;
        
        if (-33.2f >= this.transform.position.x)
        {
            transform.position = new Vector2(33.2f, 0);
        }
    }
}
