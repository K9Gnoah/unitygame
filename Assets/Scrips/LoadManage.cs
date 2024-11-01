using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManage : MonoBehaviour
{
    public CharDatabase charDB;
    public MapDatabase mapDB; // Reference to the map database
    public SpriteRenderer artworkMapSprite; // Reference to the artwork sprite
    public SpriteRenderer artworkChar1Sprite;
    public SpriteRenderer artworkChar2Sprite;

    private int mapSelectedOption = 0; // The selected map option
    private int selectedP1Option = 0;
    private int selectedP2Option = 0;


    private void Start()
    {
        if (!PlayerPrefs.HasKey("selectedMap"))
        {
            mapSelectedOption = 0;
        }
        else
        {
            Load();
        }
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
        UpdateMap(mapSelectedOption);
        UpdateChar1(selectedP1Option);
        UpdateChar2(selectedP2Option);
    }


    private void UpdateMap(int selectedOption)
    {
        Map map = mapDB.getMap(selectedOption);
        artworkMapSprite.sprite = map.mapSprite;
    }
    private void UpdateChar1(int selectedP1Option)
    {
        Char charecter1 = charDB.GetChar(selectedP1Option);
        artworkChar1Sprite.sprite = charecter1.charSprite;
    }
    private void UpdateChar2(int selectedP2Option)
    {
        Char charecter2 = charDB.GetChar(selectedP2Option);
        artworkChar2Sprite.sprite = charecter2.charSprite;
    }

    private void Load()
    {
        mapSelectedOption = PlayerPrefs.GetInt("selectedMap");
        selectedP1Option = PlayerPrefs.GetInt("selectedP1");
        selectedP2Option = PlayerPrefs.GetInt("selectedP2");
    }
}
