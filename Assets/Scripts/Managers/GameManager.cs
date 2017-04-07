using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script will keep track of the gamestates:
/// Selecting character
/// Moving character
/// Doing action with character
/// Animating character
/// </summary>

public class GameManager : singleton<GameManager>
{
    public enum GameStates
    {
        Selecting = 0,
        Moving = 1,
        Action = 2,
        Attacking = 3,
        Animating = 4,
        AIMove = 5,
        GameOver = 6,
        Initialize = 7,
    };


    private int m_roundCount;
    private int m_currentState;

    public int GameState { get { return m_currentState; } set { m_currentState = value; print("Game State: " + m_currentState); } }

    private void Start()
    {
        m_currentState = (int)GameStates.Initialize;
    }

    public void EndAITurn(List<MoveableCharacter> EnemyTeam)
    {
        for (int i = 0; i < EnemyTeam.Count; i++)
        {
            EnemyTeam[i].m_hasMoved = false;
        }

        //set gamestate to 0 (selecting)
        GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;

        //invoke the reset team function;
        SelectionManager.Instance.PlayerTeam.ResetTeam();
    }

    public void DoGameOver(string gameOverText)
    {
        print("GameComplete: " + gameOverText);

        MenuManagement.Instance.DisplayGameOver(gameOverText);
    }

    public void InitializeNewGame()
    {
        //Initialize the message log;
        SelectionManager.Instance.log.Initialize();

        //Initialize the map
        Map.Instance.InitializePieces();

        //Initialize the selection manager
        SelectionManager.Instance.Iniitialize();

        //Initiialize the ai;
        SelectionManager.Instance.EnemyTeam.GetComponent<EnemyAI>().Initialize();

        //Set the game state to 0 (selecting)
        GameState = (int)GameStates.Selecting;
    }

}
