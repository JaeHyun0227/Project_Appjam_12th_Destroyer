using UnityEngine;
using System.Collections;

public class BuildingSpawn : MonoBehaviour {

    public float spawnTime;
    
    public Building[] SmallBuilding = new Building[5];
    public Building[] MiddleBuilding = new Building[5];
    public Building[] BigBuilding = new Building[5];

    public Building SmallBuildingOrigin;
    public Building MiddleBuildingOrigin;
    public Building BigBuildingOrigin;


    // Use this for initialization
    void Start() {

        spawnTime = 3;

        for (int i = 0; i < SmallBuilding.Length; i++)
        {
            SmallBuilding[i] = GameObject.Instantiate(SmallBuildingOrigin) as Building;
            SmallBuilding[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MiddleBuilding.Length; i++)
        {
            MiddleBuilding[i] = GameObject.Instantiate(MiddleBuildingOrigin) as Building;
            MiddleBuilding[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < BigBuilding.Length; i++)
        {
            BigBuilding[i] = GameObject.Instantiate(BigBuildingOrigin) as Building;
            BigBuilding[i].gameObject.SetActive(false);
        }

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);


        switch (Random.Range(1, 4))
        {
            case 1:
                for (int i=0;i< SmallBuilding.Length;i++)
                {
                    if (!SmallBuilding[i].gameObject.activeInHierarchy)
                    {
                        SmallBuilding[i].setBuilding();
                        break;
                    }
                }
                break;

            case 2:
                for (int i = 0; i < MiddleBuilding.Length; i++)
                {
                    if (!MiddleBuilding[i].gameObject.activeInHierarchy)
                    {
                        MiddleBuilding[i].setBuilding();
                        break;
                    }
                }
                break;

            case 3:
                for (int i = 0; i < BigBuilding.Length; i++)
                {
                    if (!BigBuilding[i].gameObject.activeInHierarchy)
                    {
                        BigBuilding[i].setBuilding();
                        break;
                    }
                }
                break;
        }
          
        StartCoroutine(Spawn());
    }

}
