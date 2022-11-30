using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾��� ü�°� ���õ� �ڵ忡 ���� ��ũ��Ʈ
public class Player_Health : MonoBehaviour
{

    //�����̴��� �ޱ�
    public Slider HungrySlider;
    public Slider WaterSlider;
    public Slider MentalSlider;

    public float dotTime; //�÷��̾ ���������� ���ظ� �Դ� �ð�
    float timeSpan;  //�ð��� ���� ��ų ��
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
        //hp�� ü�¹ٷ� ��ȯ
        MentalSlider.value = MentalCurrentHp / MentalMaxHp;
        WaterSlider.value = WaterCurrentHp / WaterMaxHp;
        HungrySlider.value = HungryCurrentHp / HungryMaxHp;

        //hp�� 0���� �Ʒ��� ��������
        if (MentalCurrentHp <= 0 || WaterCurrentHp <= 0 || HungryCurrentHp <= 0)
        {
            isDead = true;
            Game_Manager.instance.GameOver(); // ���� ����ó��
        }

        timeSpan += Time.deltaTime;  // ��� �ð��� timeSpan�� ����
        if (timeSpan > dotTime)  // ��� �ð��� Ư�� �ð��� ���� Ŀ���� ��� �÷��̾��� Hp�� ���ҽ�Ų��.
        {
            IncDegHp("Mental", -1);  
            timeSpan = 0; //timespan���� �ʱ�ȭ ��Ų��.
        }
    }

    public void IncDegHp(string HpName, float HpValue) //�÷��̾� ü�� ���� ó��
    {
        switch (HpName)
        {
            case "Mental" :
                if(MentalCurrentHp + HpValue > MentalMaxHp) //ü�� ȸ�� ���� �ִ�ü�º��� ũ�ٸ�
                {
                    MentalCurrentHp = MentalMaxHp;
                }
                else
                {
                    MentalCurrentHp += HpValue;
                }
                break;
            case "Water":
                if (WaterCurrentHp + HpValue > WaterMaxHp) //ü�� ȸ�� ���� �ִ�ü�º��� ũ�ٸ�
                {
                    WaterCurrentHp = WaterMaxHp;
                }
                else
                {
                    WaterCurrentHp += HpValue;
                }
                break;
            case "Hungry":
                if (HungryCurrentHp + HpValue > HungryMaxHp) //ü�� ȸ�� ���� �ִ�ü�º��� ũ�ٸ�
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
