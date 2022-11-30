using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class PlayerData // 데이터를 저장할 클래스
{
    public string UserName; // 유저 이름
    public float HungryHp = 100f; // 플레이어 hp
    public float WaterHp = 100f; // 플레이어 hp
    public float MentalHp = 100f; // 플레이어 hp
    public Vector3 playerPos; // 플레이어 위치
    public int Date = 1; // 게임 날자
    public float curTime = 0f; //게임시간을 누적시킨값
    //public Vector3 playerRot; 
}



public class DataManager : MonoBehaviour
{
    public static DataManager instance; //싱글톤
    public PlayerData nowPlayer = new PlayerData();
    public string path; // 저장경로 변수 생성
    public int nowSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        path = Application.persistentDataPath + "/"; //유니티 지정 기본경로로 경로 설정
    }

    
    void Start()
    {
        
    }

    public void SaveData() //Json 형식으로 저장
    {
        string data = JsonUtility.ToJson(nowPlayer, true); 
        File.WriteAllText(path + nowSlot.ToString(), data);
    }

    public void LoadData() 
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }
    
    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }
}
