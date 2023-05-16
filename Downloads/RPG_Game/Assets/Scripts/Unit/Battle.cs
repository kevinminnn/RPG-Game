using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public GeneralBase Base { get; set; }

    public int Level { get; set; }

    public int HP { get; set; }
    
    public List<Moves> Moves { get; set; }

    public Battle(GeneralBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHP;

        Moves = new List<Moves>();

        foreach (var move in Base.Moves)
        {
            if (move.Level <= Level)
                Moves.Add(new Moves(move.Base, move.Level));

            if (Moves.Count >= 4)
                break;
        }
    }

    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Atk * Level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Def * Level) / 100f) + 5; }
    }

    public int Energy
    {
        get { return Mathf.FloorToInt((Base.Energy * Level) / 100f) + 5; }
    }


    public DamageDetails TakeDamage(Moves move, Battle attacker)
    {
        float criticalHit = 1f;

        if (Random.value * 100f <= 6.25f)
        {
            criticalHit = 2f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Element, this.Base.Element);

        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = criticalHit,
            Fainted = false

        };

        float modifiers = Random.Range(0.85f, 1f) * type * criticalHit;
        float a = (2 * attacker.Level * 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;

        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }


    public Moves GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float Type { get; set; }

}
