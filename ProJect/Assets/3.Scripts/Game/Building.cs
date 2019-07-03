using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{

    public enum BuildingState
    {
        SMALL,
        MIDDLE,
        BIG
    };

    public Collider2D coll;

    public BuildingState state;

    public float speed = 1;

    public float Health;
    float curHealth; // 체력
    
    int Floor; // 층

    public GameObject exp;

    void Update()
    {
        this.transform.position += Vector3.left * (speed + GameManager.GetInstance().plusSpeed)  * Time.deltaTime;

        if (transform.position.x < -10) gameObject.SetActive(false);
    }

    public void setBuilding()
    {
        float PosY = 0;
        switch (state)
        {
            case BuildingState.SMALL:
                Floor = Random.Range(0, 2);
                PosY = GameManager.GetInstance().floorPosy[Floor];
                break;

            case BuildingState.MIDDLE:
                Floor = Random.Range(0, 1);
                switch (Floor)
                {
                    case 0:
                        PosY = Mathf.Lerp(GameManager.GetInstance().floorPosy[0], GameManager.GetInstance().floorPosy[1], 0.5f);
                        break;
                    case 1:
                        PosY = Mathf.Lerp(GameManager.GetInstance().floorPosy[1], GameManager.GetInstance().floorPosy[2], 0.5f);
                        break;
                }
                Floor = 1;
                break;

            case BuildingState.BIG:
                Floor = 1;
                PosY = 0;
                break;
        }

        gameObject.SetActive(true);
        Vector2 Pos = new Vector2(10, PosY);

        curHealth = Health;
        transform.position = Pos;

    }

    public void DamageBuilding(float damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(GameManager.GetInstance().BoomSound, Vector2.zero);//폭팔음

            exp.SetActive(true);
            StartCoroutine(expDelay());

            switch (state)
            {
                case BuildingState.SMALL:
                    GameManager.GetInstance().PlusScore(100);
                    break;
                case BuildingState.MIDDLE:
                    GameManager.GetInstance().PlusScore(200);
                    break;
                case BuildingState.BIG:
                    GameManager.GetInstance().PlusScore(400);
                    break;
            }
            Debug.Log(Floor);
            GameManager.GetInstance().ItemDrop(new Vector2(transform.position.x, Floor));
        }
    }

    IEnumerator expDelay()
    {
        coll.enabled = false;

        float speedSave = speed;
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        exp.SetActive(false);
        gameObject.SetActive(false);
        speed = speedSave;
        coll.enabled = true;    
    }
}
