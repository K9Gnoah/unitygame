using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharDatabase : ScriptableObject
{
    public Char[] charecter;
    public int CharCount
    {
        get { return charecter.Length; }
    }

    public Char GetChar(int index)
    {
        return charecter[index];
    }

}
