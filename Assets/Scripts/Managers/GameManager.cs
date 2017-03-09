using UnityEngine;
using System.Collections;

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
    };


    private int m_roundCount;
    private int m_currentState;

    public int GameState { get { return m_currentState; } set { m_currentState = value; print("Game State: " + m_currentState); } }

    private void Start()
    {
        m_currentState = (int)GameStates.Selecting;
    }

    private void Update()
    {
        //print(m_currentState);
    }
}
