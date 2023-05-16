using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { OpenWorld, BattleAction }

public class GameController : MonoBehaviour
{
    [SerializeField] CharacterMovement playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera openCamera;

    GameState state;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle()
    {
        state = GameState.BattleAction;
        battleSystem.gameObject.SetActive(true);
        openCamera.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }

    void EndBattle(bool win)
    {
        state = GameState.OpenWorld;
        battleSystem.gameObject.SetActive(false);
        openCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.OpenWorld)
        {
            playerController.HandleUpdate();
        }

        else if (state == GameState.BattleAction) 
        {
            battleSystem.HandleUpdate();
        }
    }
}
