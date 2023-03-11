using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Level",menuName ="Level")]
public class Level : ScriptableObject
{
    public Goal goal;
    public GameGrid GameGrid;
    public int moves;
}
