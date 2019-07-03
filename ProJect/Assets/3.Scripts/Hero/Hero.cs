using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    


    //체력
    public int Health = 3;
    public GameObject boom;
    public Collider2D coll;

    //층
    public int floor;//층 0~2;

    //움직임
    public float upDownSpeed;//위아래로 움직이는 속도
    
    float StartY;//움직이기 시작한 위치y
    int endFloor;//끝나는 층
    float progress=2;//움직임 진행도

    //레이저
    public float lazerDamage;
    bool isLazer = false;
    bool isCoolTime = false;
    public float lazerTimeMax;//레이저 최대 발사시간
    public float lazerCooltimeSpeed;//레이저 쿨타임 스피드
    float lazerTime = 0;//레이저 타이머
    public LineRenderer LazerRen;
    public AudioSource lazersound;

    public LayerMask lazerMask;//레이어 마스크
    public LazerTrail lazertrail;
    public LazerTrail[] lazertrails = new LazerTrail[10];//레이저 자국
    public Transform LazersParent;//레이저들의 부모

    //미사일
    public int missileMax;//미사일 최대개수
    int FlyingMissiles=0;//날라가는 미사일 수;
    public Missile missile;//미사일 원본
    public float missileCoolTime;
    public Missile[] missiles = new Missile[10];//레이저 자국
    public Transform MissilesParent;//미사일들의 부모

    public AudioClip MissileSound;


    void OnEnable()
    {
        //자리 잡기
        Vector2 pos = transform.position;
        pos.y = GameManager.GetInstance().floorPosy[floor];
        transform.position = pos;

        //레이저 자국들 미리 만들기
        for(int i=0;i< lazertrails.Length;i++)
        {
            lazertrails[i] = GameObject.Instantiate(lazertrail) as LazerTrail;
            lazertrails[i].transform.parent = LazersParent;
            lazertrails[i].gameObject.SetActive(false);
        }

        //미사일 미리 만들기
        for (int i = 0; i < missiles.Length; i++)
        {
            missiles[i] = GameObject.Instantiate(missile) as Missile;
            missiles[i].hero = this;
            missiles[i].transform.parent = MissilesParent;
            missiles[i].gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Moving();
        LazerTimer();
    }

    public void Move(int dir)//움직일 층 체크
    {
        switch (dir)
        {
            case 1:
                if (floor < 2)
                {
                    endFloor = floor + 1;
                    floor = floor + 1;
                    progress = 0;
                }
                break;

            case -1:
                if (floor > 0)
                {
                    endFloor = floor - 1;
                    floor = floor - 1;
                    progress = 0;
                }
                break;
        }
        StartY = transform.position.y;
    }
    void Moving()//움직임
    {
        if (progress > 1) return;

        
        float Y = Mathf.Lerp(StartY, GameManager.GetInstance().floorPosy[endFloor], progress);

        Vector2 pos = transform.position;
        pos.y = Y;
        transform.position = pos;

        progress += Time.deltaTime * upDownSpeed;
    }

    public void Missile(Vector2 pos)
    {
        AudioSource.PlayClipAtPoint(MissileSound, Vector2.zero);//미사일 소리 재생

        if (FlyingMissiles >= missileMax) return;
            FlyingMissiles++;
        UIManager.GetInstance().Missile(FlyingMissiles,missileMax);
        
        for (int i = 0; i < missiles.Length; i++)
        {
            if (!missiles[i].gameObject.activeInHierarchy)
            {
                missiles[i].SetMissile(transform.position,pos);
                StartCoroutine(MissileCoolTime());
                break;
            }
        }
    }
    
    IEnumerator MissileCoolTime()
    {
        yield return new WaitForSeconds(missileCoolTime);
        FlyingMissiles--;
        UIManager.GetInstance().Missile(FlyingMissiles, missileMax);
    }

    public void Lazer(Vector3 pos)
    {
        if (isCoolTime) {return;}

        lazersound.mute = false;

        isLazer = true;

        LazerRen.gameObject.SetActive(true);

        LazerRen.SetPosition(0, LazerRen.transform.position);
        LazerRen.SetPosition(1, pos);

        Collider2D coll = Physics2D.OverlapPoint(pos, lazerMask);
        if(coll)
        {
            coll.GetComponent<Building>().DamageBuilding(Time.deltaTime * lazerDamage);
            for (int i=0;i< lazertrails.Length;i++)
            {
                if (!lazertrails[i].gameObject.activeInHierarchy)
                {
                    lazertrails[i].transform.parent = coll.transform;
                    lazertrails[i].SetTrail(pos);
                    break;
                }
            }
        }
        lazerTime += Time.deltaTime;

        UIManager.GetInstance().RazerBar(lazerTime, lazerTimeMax);
        if (lazerTime > lazerTimeMax) { LazerEnd(); }
    }

    void LazerTimer()
    {
        if (isLazer) return;
        if (isCoolTime && lazerTime > 0)
        {
            lazerTime -= Time.deltaTime * lazerCooltimeSpeed;
            UIManager.GetInstance().RazerBar(lazerTime, lazerTimeMax);
        }
        else
            isCoolTime = false;
    }
    public void LazerEnd()
    {
        lazersound.mute = true;
        isLazer = false;
        LazerRen.gameObject.SetActive(false);

        isCoolTime = true;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Item")) return;

        coll.GetComponent<Building>().DamageBuilding(100);

        Health--;
        if(Health <= 0)
        {
            UIManager.GetInstance().Result();
            StartCoroutine(Boom());
            return;
        }
        UIManager.GetInstance().Health(Health);
    }

    IEnumerator Boom()
    {
        coll.enabled = false;
        boom.SetActive(true);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    public void AddHealth()
    {
        
        if (Health < 3)
        {
            Health++;
            UIManager.GetInstance().Health(Health);
        }
    }

    public void AddMissileMax() {
        if (missiles.Length > missileMax) {
            missileMax++;
            UIManager.GetInstance().Missile(FlyingMissiles, missileMax);
        }
    }
    
    public void AddLazerTime()
    {
        lazerTimeMax += 0.5f;
        UIManager.GetInstance().RazerBar(lazerTime, lazerTimeMax);
    }
}
