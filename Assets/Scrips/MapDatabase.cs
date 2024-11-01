using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapDatabase : ScriptableObject
{
    public Map[] map;
    public int mapCount
    {
        get { return map.Length; }
    }

    public Map getMap(int index)
    {
        return map[index];
    }
}
