using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Main_Menu : MonoBehaviour
{
    //�гε��� �Է¹޴´�.
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
        for(int i = 0; i < 3; i++) //���Կ� ���� �����Ͱ� �ִ��� ���� Ȯ��
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
                slotText[i].text = "����ִ� ����";
            }
        }
        DataManager.instance.DataClear();
    }

    //���Ӿ� �ҷ�����
    public void LoadGameScene()
    {
        if (!savefile[DataManager.instance.nowSlot])
        {
            DataManager.instance.nowPlayer.UserName = newPlayerName.text;
            DataManager.instance.SaveData();
        }
        SceneManager.LoadScene("Game_Scene");
    }

    //���� ���� ��ư ó��
    public void QuitGame()
    {
        Application.Quit();
    }

    //�ɼ� ��ư ó�� 
    public void Option()
    {
        OptionPanel.SetActive(true);
    }

    //�ɼ� �г� ����
    public void OptionPanelOff()
    {
        OptionPanel.SetActive(false);
    }

    //���� ���� ��ư ó��
    public void GameStart()
    {
        StartGamePanel.SetActive(true);
    }

    //���ӽ����г� ����
    public void StartGamePanelOff()
    {
        StartGamePanel.SetActive(false);
    }

    //��ŷ �г� ó��
    public void Ranking()
    {
        RankingPanel.SetActive(true);
    }

    //��ŷ �г� ����
    public void RankingPanelOff()
    {
        RankingPanel.SetActive(false);
    }

    //�̾��ϱ� Ŭ����
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
        
        if (savefile[number])     //���� ������ ������
        {
            DataManager.instance.LoadData();
            LoadGameScene();
        }
        else // ������
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
