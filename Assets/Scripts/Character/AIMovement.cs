using UnityEngine;
using System.Collections;
/// <summary>
/// This script is responsible for AI behavior, i.e moving and attacking with each character of the AI team.
/// 
///Pseudocode:
///Find the nearest playercharacter
///move within attack range
///set didmove to true
///atack player character
///set didattack to true
///when all characters didmove & didattack, set gamestate to selecting and increment the round count;
/// </summary>

public class AIMovement : MonoBehaviour
{
    public SelectableCharacters PlayerTeam;
    public SelectableCharacters EnemyTeam;

    public CharacterStats current;

    private void Start()
    {
        PlayerTeam = SelectionManager.Instance.PlayerTeam;
        EnemyTeam = SelectionManager.Instance.EnemyTeam;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.AIMove)
        {
            //Set target for EnemyTeam.Team[i] to FindclosestTarget();
            //Move to (FindClosestTarget(Enemyteam.Team[i].m_currentPos;
            //Attaack target
            for (int i = 0; i < PlayerTeam.TeamSize; i++)
            {
                //SET TARGET
                current = EnemyTeam.Team[i].GetComponent<CharacterStats>();
                current.m_target = FindClosetsTarget(EnemyTeam.Team[i].m_CurrentLocation);

                //MOVE TO TARGET
                EnemyTeam.Team[i].m_Destination = current.m_target.m_CurrentLocation;
                EnemyTeam.Team[i].m_isSelected = true;
                EnemyTeam.Team[i].m_moving = true;

                //ATTACK TARGET
                current.AttackTarget(current.m_target.GetComponent<CharacterStats>(), 0);
            }

        }

    }

    private MoveableCharacter FindClosetsTarget(Vector2 StartLocation)
    {
        //count the steps to each player character;
        //return the smallest value of steps;


        return PlayerTeam.Team[0];
    }

}
