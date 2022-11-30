using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SpawnEnemy : MonoBehaviour
{
    public GameObject Enemy;

    public Transform[] spawnPoints; //스폰위치들
    
    private int wave = 1;   // 시간이 흐름에 따라 수를 늘릴 
    private int day;    //게임 날자 받아올 변수

    public GameObject[] enemies;

    public static Game_SpawnEnemy instance;

    private void Awake()
    {
        instance = this;
    }
    public void CreateEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //좌표 4개중 한곳을 랜덤으로 지정
        for(int i = 0; i < wave; i++) //wave의 수만큼 반복문 실행
        {
            Instantiate(Enemy, spawnPoint.position, Quaternion.identity); //생성
            StartCoroutine(WaitForCreate());    //같은 위치에 겹치지 않게 잠시 대기
        }
        wave++; 
    }

    IEnumerator WaitForCreate() 
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void Update()
    {
    
    }
}
