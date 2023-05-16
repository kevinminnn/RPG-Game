using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] Action playerStats;
    [SerializeField] Action enemyStats;
    [SerializeField] BattleDialogue dialog;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerStats.SetData(playerUnit.entity);
        enemyStats.SetData(enemyUnit.entity);

        dialog.SetMoveNames(playerUnit.entity.Moves);
        yield return dialog.TypeDialog($"A wild {enemyUnit.entity.Base.Name} appeared!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialog.TypeDialog("Choose an action:"));
        dialog.EnableActionSelector(true);

    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialog.EnableActionSelector(false);
        dialog.EnableDialogText(false);
        dialog.EnableMoveSelector(true);

    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var selectedMove = playerUnit.entity.Moves[currentMove];
        var moveBase = selectedMove.Base;

        yield return dialog.TypeDialog($"{playerUnit.entity.Base.Name} used {moveBase.Name}!");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();

        var damageDetails = enemyUnit.entity.TakeDamage(selectedMove, playerUnit.entity);
        yield return enemyStats.UpdateHP();
        yield return ShowDamageDetails(damageDetails);


        if (damageDetails.Fainted)
        {
            yield return dialog.TypeDialog($"{enemyUnit.entity.Base.Name} Fainted!");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }


    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.entity.GetRandomMove();
        yield return dialog.TypeDialog($"{enemyUnit.entity.Base.Name} used {move.Name}!");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();

        var damageDetails = playerUnit.entity.TakeDamage(move, enemyUnit.entity);
        yield return playerStats.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialog.TypeDialog($"{playerUnit.entity.Base.Name} Fainted!");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }

        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialog.TypeDialog("A critical hit!");
        }

        if (damageDetails.Type > 1f)
        {
            yield return dialog.TypeDialog("It's super effective!");
        }

        else if (damageDetails.Type <= 1f) 
        {
            yield return dialog.TypeDialog("It's not very effective.");

        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }

        else if (state == BattleState.PlayerMove) {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction < 1)
            {
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }

        dialog.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == 0)
            {
                PlayerMove();
            }

            else if (currentAction == 1) 
            {
                //Run
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.entity.Moves.Count - 1)
            {
                ++currentMove;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.entity.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }

        dialog.UpdateMoveSelection(currentMove, playerUnit.entity.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialog.EnableMoveSelector(false);
            dialog.EnableDialogText(true);

            StartCoroutine(PerformPlayerMove());
        }
    }

}
