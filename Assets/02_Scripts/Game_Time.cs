using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//게임에서의 시간을 관리 하는 스크립트 
public class Game_Time : MonoBehaviour
{

    public Text dateText; //출력할 텍스트
    public int date; // 현재 날자 변수
    public float goalDayTime; // 날자 변환의 목표 초
    public float curDayTime; // 현재 시간을 누적시킬 변수

    public static Game_Time instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!(DataManager.instance.nowPlayer.Date.Equals(date)&& DataManager.instance.nowPlayer.curTime.Equals(curDayTime))) //시간이 초기값이 아닐경우 저장값 불러와서 변수에 넣기
        {
            date = DataManager.instance.nowPlayer.Date;
            curDayTime = DataManager.instance.nowPlayer.curTime;
            dateText.text = "Day " + date; // 텍스트 화면 출력
        }
    }


    void Update()
    {
        //일정 시간이 차면 날자를 올림
        curDayTime += Time.deltaTime;
        if (curDayTime > goalDayTime)
        {
            date++; //날자 올림
            if(date % 3 == 0)
            {
                Game_SpawnEnemy.instance.CreateEnemy();
            }
            Game_Score.instance.dayCnt++; // 점수용 변수도 같이 올림
            dateText.text = "Day " + date; // 텍스트 화면 출력
            curDayTime = 0f; // 시간 누적값 초기화
        }
    }
}

