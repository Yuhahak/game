using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 플레이어의 체력과 관련된 코드에 대한 스크립트
public class Player_Health : MonoBehaviour
{

    //슬라이더를 받기
    public Slider HungrySlider;
    public Slider WaterSlider;
    public Slider MentalSlider;

    public float dotTime; //플레이어가 지속적으로 피해를 입는 시간
    float timeSpan;  //시간을 누적 시킬 값
    public float MentalMaxHp;
    public float MentalCurrentHp;
    public float WaterMaxHp;
    public float WaterCurrentHp;
    public float HungryMaxHp;
    public float HungryCurrentHp;

    public bool isDead = false;


    public static Player_Health instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timeSpan = 0f;
        if(!(DataManager.instance.nowPlayer.MentalHp.Equals(MentalCurrentHp) && DataManager.instance.nowPlayer.WaterHp.Equals(WaterCurrentHp) && DataManager.instance.nowPlayer.HungryHp.Equals(HungryCurrentHp)))
        {
            MentalCurrentHp = DataManager.instance.nowPlayer.MentalHp;
            WaterCurrentHp = DataManager.instance.nowPlayer.WaterHp;
            HungryCurrentHp = DataManager.instance.nowPlayer.HungryHp;
        }
    }

    void Update()
    {
        //hp를 체력바로 변환
        MentalSlider.value = MentalCurrentHp / MentalMaxHp;
        WaterSlider.value = WaterCurrentHp / WaterMaxHp;
        HungrySlider.value = HungryCurrentHp / HungryMaxHp;

        //hp가 0보다 아래로 내려가면
        if (MentalCurrentHp <= 0 || WaterCurrentHp <= 0 || HungryCurrentHp <= 0)
        {
            isDead = true;
            Game_Manager.instance.GameOver(); // 게임 오버처리
        }

        timeSpan += Time.deltaTime;  // 경과 시간을 timeSpan에 누적
        if (timeSpan > dotTime)  // 경과 시간이 특정 시간이 보다 커졋을 경우 플레이어의 Hp를 감소시킨다.
        {
            IncDegHp("Mental", -1);  
            timeSpan = 0; //timespan값을 초기화 시킨다.
        }
    }

    public void IncDegHp(string HpName, float HpValue) //플레이어 체력 가감 처리
    {
        switch (HpName)
        {
            case "Mental" :
                if(MentalCurrentHp + HpValue > MentalMaxHp) //체력 회복 값이 최대체력보다 크다면
                {
                    MentalCurrentHp = MentalMaxHp;
                }
                else
                {
                    MentalCurrentHp += HpValue;
                }
                break;
            case "Water":
                if (WaterCurrentHp + HpValue > WaterMaxHp) //체력 회복 값이 최대체력보다 크다면
                {
                    WaterCurrentHp = WaterMaxHp;
                }
                else
                {
                    WaterCurrentHp += HpValue;
                }
                break;
            case "Hungry":
                if (HungryCurrentHp + HpValue > HungryMaxHp) //체력 회복 값이 최대체력보다 크다면
                {
                    HungryCurrentHp = HungryMaxHp;
                }
                else
                {
                    HungryCurrentHp += HpValue;
                }
                break;
        }
    }
}
