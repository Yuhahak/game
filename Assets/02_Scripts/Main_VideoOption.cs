using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    
    public int resolutionNum; //������ �ɼ��� ������ ����

    void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        /*//�ֻ����� 60�� ���� �޾ƿ���
        for (int i = 0; i <Screen.resolutions.Length; i++)
        {
            if(Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }*/

        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear(); //�ɼ� �ʱ�ȭ

        int optionNum = 0;
        //�ݺ������� �迭�� �ɼ� ����
        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "X" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);
            if(item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }

        //���ΰ�ħ
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull) //���������� ��üȭ���� ����
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ApplyBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
