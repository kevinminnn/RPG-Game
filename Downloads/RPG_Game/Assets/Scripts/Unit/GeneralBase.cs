using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle", menuName = "Battle/Create new entity")]

public class GeneralBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string desc;

    [SerializeField] Sprite front;
    [SerializeField] Sprite back; 

    [SerializeField] Type element;

    [SerializeField] int maxHp;
    [SerializeField] int atk;
    [SerializeField] int def;
    [SerializeField] int energy;

    [SerializeField] List<Moves> moves;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return desc; }
    }

    public Sprite frontSprite
    {
        get { return front; }
    }

    public Sprite backSprite
    {
        get { return back; }
    }

    public Type Element
    {
        get { return element; }
    }

    public int MaxHP
    {
        get { return maxHp; }
    }

    public int Atk
    {
        get { return atk; }
    }

    public int Def
    {
        get { return def; }
    }

    public int Energy
    {
        get { return energy; }
    }

    public List<Moves> Moves
    {
        get { return moves; }
    }
}

[System.Serializable]
public class Moves
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public Moves(MoveBase moveBase, int level)
    {
        this.moveBase = moveBase;
        this.level = level;
    }

    public MoveBase Base
    {
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }

    public string Name
    {
        get { return moveBase.Name; }
    }
}

public enum Type
{
    Pyro,
    Hydro,
    Cryo,
    Electro
}

public class TypeChart
{
    static float[][] chart = {
        new float[] { 1f, 1.2f, 1.2f, 1.2f },
        new float[] { 1.2f, 1f, 1.2f, 0.5f },  
        new float[] { 1.2f, 0.25f, 1f, 1.2f },  
        new float[] { 1.2f, 0.5f, 1.2f, 1f }  
    };

    public static float GetEffectiveness(Type attackType, Type defenseType)
    {
        int row = (int)attackType;
        int col = (int)defenseType;

        return chart[row][col];
    }
}

