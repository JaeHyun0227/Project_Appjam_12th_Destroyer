using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public GameManager.ItemState IS;

    public float speed;
    

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, GameManager.GetInstance().hero.transform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (IS)
        {
            case GameManager.ItemState.HEALTH:
                GameManager.GetInstance().hero.AddHealth();
                break;
            case GameManager.ItemState.MISSILE:
                GameManager.GetInstance().hero.AddMissileMax();
                break;
            case GameManager.ItemState.LAZER:
                GameManager.GetInstance().hero.AddLazerTime();
                break;
        }
        gameObject.SetActive(false);
    }
}
