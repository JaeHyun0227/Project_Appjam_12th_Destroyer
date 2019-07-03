using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }

        return instance;
    }

    public enum ItemState{
        HEALTH,
        MISSILE,
        LAZER
    };

    public AudioClip BoomSound;

    public float[] floorPosy = new float[3];//층 의 위치들

    public Hero hero;

    int Rand;
    ItemState RandomItem;
    public Item[] Items = new Item[10];
    public Sprite[] ItemSprite = new Sprite[3];
    public Item ItemOrigin;

    public float plusSpeed = 0;

    public int Score = 0;

    void Start()
    {
        for(int i=0;i< Items.Length;i++)
        {
            Items[i] = Instantiate(ItemOrigin) as Item;
            Items[i].gameObject.SetActive(false);
        }
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(9);
        PlusScore(100);
        plusSpeed += 0.5f;

        StartCoroutine(Timer());
    }

    public void ItemDrop(Vector2 location)
    {
        Rand = (int)Random.Range(0, 100);

        if (Rand <= 30)
        {
            RandomItem = (ItemState)Random.Range(0, 3);

            for(int i=0;i<Items.Length;i++)
            {
                if (!Items[i].gameObject.activeInHierarchy)
                {
                    Debug.Log("sadadasd");
                    Items[i].GetComponent<SpriteRenderer>().sprite = ItemSprite[(int)RandomItem];
                    Items[i].IS = RandomItem;

                    location.y = floorPosy[(int)location.y];
                    Items[i].transform.position = location;

                    Items[i].gameObject.SetActive(true);
                    return;
                }
            }
        }

    }

    public void PlusScore(int point)
    {
        Score += point;
        UIManager.GetInstance().Score(Score);
    }
}
