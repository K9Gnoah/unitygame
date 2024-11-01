using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagement : MonoBehaviour
{
    private static CharacterManagement instance;
    public static CharacterManagement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CharacterManagement();
            }
            return instance;
        }
    }

    public int SelectedPlayer1 { get; set; }
    public int SelectedPlayer2 { get; set; }
}
