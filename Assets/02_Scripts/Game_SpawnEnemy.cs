using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SpawnEnemy : MonoBehaviour
{
    public GameObject Enemy;

    public Transform[] spawnPoints; //������ġ��
    
    private int wave = 1;   // �ð��� �帧�� ���� ���� �ø� 
    private int day;    //���� ���� �޾ƿ� ����

    public GameObject[] enemies;

    public static Game_SpawnEnemy instance;

    private void Awake()
    {
        instance = this;
    }
    public void CreateEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //��ǥ 4���� �Ѱ��� �������� ����
        for(int i = 0; i < wave; i++) //wave�� ����ŭ �ݺ��� ����
        {
            Instantiate(Enemy, spawnPoint.position, Quaternion.identity); //����
            StartCoroutine(WaitForCreate());    //���� ��ġ�� ��ġ�� �ʰ� ��� ���
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
