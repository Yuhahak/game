using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game_Score : MonoBehaviour // ���� ���� ������ ȯ���ϴ� ���ھ� ����
{
    public Text GameOverText; 
    float totalScore; // �� ����
    public int killCnt  = 1; //���� ���� ��
    public int dayCnt; //���ڰ� ���� ��

    public static Game_Score instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dayCnt = Game_Time.instance.date; // ���� ������ ���� ���� ���� �޾ƿ���
    }

    void Update()
    {
        
    }

    public void PrintScore() //���� ���
    {
        totalScore = (killCnt * 1.2f) * (dayCnt * 1.2f); 
        GameOverText.text = "���� : " + (int)totalScore;
    }
}
