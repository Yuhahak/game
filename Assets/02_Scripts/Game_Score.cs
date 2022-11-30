using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game_Score : MonoBehaviour // 최종 게임 점수를 환산하는 스코어 보드
{
    public Text GameOverText; 
    float totalScore; // 총 점수
    public int killCnt  = 1; //적을 죽인 수
    public int dayCnt; //날자가 지난 수

    public static Game_Score instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dayCnt = Game_Time.instance.date; // 날자 변수에 게임 현재 날자 받아오기
    }

    void Update()
    {
        
    }

    public void PrintScore() //점수 출력
    {
        totalScore = (killCnt * 1.2f) * (dayCnt * 1.2f); 
        GameOverText.text = "점수 : " + (int)totalScore;
    }
}
