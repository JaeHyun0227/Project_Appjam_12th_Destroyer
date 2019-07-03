using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public Hero hero;

    public float speed;//스피드
    public float Damage;//데미지

    public float guidedTime;//유도되기전에 날라가는 시간
    float Timer = 0;//유도되기전에 날라가는 시간체크

    bool isRot;//돌아가는지
    float rotZ = 0;//미사일 목표각도
    float progress;//미사일 각도 변경 과정
    public float RotSpeed;//각도변경 속도

    Vector2 arrivalPoint;//목적지

    public Collider2D coll;
    public GameObject exp;
    
    public void SetMissile(Vector2 StartPos ,Vector2 pos)
    {
        //위치와 각도 초기화
        transform.position = StartPos;
        transform.rotation = Quaternion.Euler(0,0,180);
        arrivalPoint = pos;

        gameObject.SetActive(true);
        Timer = 0;
        progress = 0;
        isRot = false;
    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);//움직임

        if (!isRot)
        {
            Timer += Time.deltaTime;
            if (Timer > guidedTime)
            {
                rotZ = (Mathf.Atan2(arrivalPoint.y - transform.position.y, arrivalPoint.x - transform.position.x) * 180f / Mathf.PI) + 180;//각도 구하기
                isRot = true;
            }
        }
        else
        {
            progress += Time.deltaTime * RotSpeed;//프로그레스
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(180, rotZ, progress));//각도 변경
            if (transform.position.x > 10 || Mathf.Abs(transform.position.y) > 5)
            {
                gameObject.SetActive(false);
            }//범위체크
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //미사일 폭팔 이펙트

        StartCoroutine(expDelay());

        coll.GetComponent<Building>().DamageBuilding(Damage);
    }
    IEnumerator expDelay()
    {
        coll.enabled = false;
        exp.SetActive(true);
        float speedSave = speed;
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        exp.SetActive(false);
        gameObject.SetActive(false);
        speed = speedSave;
        coll.enabled = true;
    }

}
