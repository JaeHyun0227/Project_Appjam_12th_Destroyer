using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    //플레이어 이동용 스와이프
    //공격용 스와이프,공격용 터치

    //영향을 받는 객체
    public Hero hero;//주인공


    //조작
    float timer = 0;//터치와 스와이프 구분을 위한 타이머
    bool isMove = false;//이동을 위한 모션인지 체크
    Vector2 StartPos = Vector2.zero;//시작점

    void Update()
    {
        swype();
    }

    void swype()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPos = Input.mousePosition;

            if (StartPos.x < Screen.width / 4)//이동용
            {
                isMove = true;
            }

            timer = 0;

        }

        if (Input.GetMouseButton(0))
        {
            if(timer > 0.2f)//0.2초가 지나면 스와이프로 간주하여 레이저 발사
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 1;
                hero.Lazer(pos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Input.mousePosition;

            if (mousePos.x < Screen.width / 4 && isMove)//이동
            {
                isMove = false;

                int dir = 0;
                if (StartPos.y < mousePos.y)//상승
                    dir = 1;
                else
                    dir = -1;

                hero.Move(dir);
            }
            else if (timer < 0.2f)//터치는 미사일
            {
                hero.Missile(Camera.main.ScreenToWorldPoint(mousePos));
            }
            hero.LazerEnd();//레이저 중지
        }

        timer += Time.deltaTime;
    }
}
