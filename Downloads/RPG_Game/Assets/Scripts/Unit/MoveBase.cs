using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Battle/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string desc;

    [SerializeField] Type element;

    [SerializeField] int power;
    [SerializeField] int cost;
    [SerializeField] int pp;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return desc; }
    }

    public Type Element
    {
        get { return element; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Cost
    {
        get { return cost; }
    }

    public int PP
    {
        get { return pp; }
    }
}
