using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Main_Menu : MonoBehaviour
{
    //패널들을 입력받는다.
    public GameObject OptionPanel;
    public GameObject RankingPanel;
    public GameObject StartGamePanel;
    public GameObject SlotPanel;
    public GameObject InputPanel;
    public Text[] slotText;
    public Text newPlayerName;
    private GameObject player;

    bool[] savefile = new bool[3];

    private void Start()
    {
        for(int i = 0; i < 3; i++) //슬롯에 기존 데이터가 있는지 유무 확인
        {
           if( File.Exists(DataManager.instance.path + $"{i}"))
            {
                savefile[i] = true;
                DataManager.instance.nowSlot = i;
                DataManager.instance.LoadData();
                slotText[i].text= DataManager.instance.nowPlayer.UserName;
            }
            else
            {
                slotText[i].text = "비어있는 슬롯";
            }
        }
        DataManager.instance.DataClear();
    }

    //게임씬 불러오기
    public void LoadGameScene()
    {
        if (!savefile[DataManager.instance.nowSlot])
        {
            DataManager.instance.nowPlayer.UserName = newPlayerName.text;
            DataManager.instance.SaveData();
        }
        SceneManager.LoadScene("Game_Scene");
    }

    //게임 종료 버튼 처리
    public void QuitGame()
    {
        Application.Quit();
    }

    //옵션 버튼 처리 
    public void Option()
    {
        OptionPanel.SetActive(true);
    }

    //옵션 패널 종료
    public void OptionPanelOff()
    {
        OptionPanel.SetActive(false);
    }

    //게임 시작 버튼 처리
    public void GameStart()
    {
        StartGamePanel.SetActive(true);
    }

    //게임시작패널 종료
    public void StartGamePanelOff()
    {
        StartGamePanel.SetActive(false);
    }

    //랭킹 패널 처리
    public void Ranking()
    {
        RankingPanel.SetActive(true);
    }

    //랭킹 패널 종료
    public void RankingPanelOff()
    {
        RankingPanel.SetActive(false);
    }

    //이어하기 클릭시
    public void ContinueClick()
    {
        SlotPanel.SetActive(true);
    }

    public void ContinueBackClick()
    {
        SlotPanel.SetActive(false);
    }
    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number;
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (savefile[number])     //저장 데이터 있을시
        {
            DataManager.instance.LoadData();
            LoadGameScene();
        }
        else // 없을시
        {
            InputPanelOpen();
        }
    }

    public void InputPanelOpen()
    {
        InputPanel.gameObject.SetActive(true);
    }

    public void InputPanelClose()
    {
        InputPanel.gameObject.SetActive(false);
    }
}
