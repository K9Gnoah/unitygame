using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharManage : MonoBehaviour
{
    public CharDatabase charDB;
    private int selectedP1Option = 0;
    private int selectedP2Option = 0;
    private bool p1Ready = false;

    public GameObject[] characters;
    public GameObject arrowP1;
    public GameObject arrowP2;

    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedP1"))
        {
            selectedP1Option = 0;
        }
        else
        {
            Load();
        }
        if (!PlayerPrefs.HasKey("selectedP2"))
        {
            selectedP2Option = 0;
        }
        else
        {
            Load();
        }
        UpdateChar1(selectedP1Option);
        UpdateChar2(selectedP2Option);
        arrowP2.SetActive(false);
    }

    void Update()
    {
        HandlePlayer1Input();
        HandlePlayer2Input();

    }

    public void NextOption()
    {
        if (!p1Ready)
        {
            selectedP1Option++;
            if (selectedP1Option >= charDB.CharCount)
            {
                selectedP1Option = 0;
            }
            UpdateChar1(selectedP1Option);
        }
        else
        {
            selectedP2Option++;
            if (selectedP2Option >= charDB.CharCount)
            {
                selectedP2Option = 0;
            }
            UpdateChar2(selectedP2Option);
        }
        Save();

    }

    public void BackOption()
    {
        if (!p1Ready)
        {
            selectedP1Option--;
            if (selectedP1Option < 0)
            {
                selectedP1Option = charDB.CharCount - 1;
            }
            UpdateChar1(selectedP1Option);
        }
        else
        {
            selectedP2Option--;
            if (selectedP2Option < 0)
            {
                selectedP2Option = charDB.CharCount - 1;
            }
            UpdateChar2(selectedP2Option);
        }
        Save();
    }

    private void UpdateChar1(int selectedP1Option)
    {
        Char p1Char = charDB.GetChar(selectedP1Option);
    }
    private void UpdateChar2(int selectedP2Option)
    {
        Char p2Char = charDB.GetChar(selectedP2Option);
    }

    private void Load()
    {
        selectedP1Option = PlayerPrefs.GetInt("selectedP1");
        selectedP2Option = PlayerPrefs.GetInt("selectedP2");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedP1", selectedP1Option);
        PlayerPrefs.SetInt("selectedP2", selectedP2Option);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void UpdateArrowPosition(int index, GameObject arrow)
    {
        arrow.transform.position = characters[index].transform.position + new Vector3(0, 270, 0);
    }

    void HandlePlayer1Input()
    {
        if (!p1Ready)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedP1Option--;
                if (selectedP1Option < 0)
                    selectedP1Option = characters.Length - 1;

                UpdateArrowPosition(selectedP1Option, arrowP1);
                UpdateChar1(selectedP1Option);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedP1Option++;
                if (selectedP1Option >= characters.Length)
                    selectedP1Option = 0;

                UpdateArrowPosition(selectedP1Option, arrowP1);
                UpdateChar1(selectedP1Option);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                p1Ready = true;
                arrowP2.SetActive(true);
                UpdateArrowPosition(selectedP2Option, arrowP2);
            }
        }
    }

    void HandlePlayer2Input()
    {
        if (p1Ready)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectedP2Option--;
                if (selectedP2Option < 0)
                    selectedP2Option = characters.Length - 1;

                UpdateArrowPosition(selectedP2Option, arrowP2);
                UpdateChar2(selectedP2Option);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectedP2Option++;
                if (selectedP2Option >= characters.Length)
                    selectedP2Option = 0;

                UpdateArrowPosition(selectedP2Option, arrowP2);
                UpdateChar2(selectedP2Option);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                PlayerPrefs.SetInt("selectedP1", selectedP1Option);
                PlayerPrefs.SetInt("selectedP2", selectedP2Option);
                ChangeScene("MapSelection");
            }
        }
    }
}