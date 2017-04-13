using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{

    //Create a list for the enemies and players,
    //using a list because we will be resizing them and lists resize like a baws!
    public List<MoveableCharacter> Enemies;
    public List<MoveableCharacter> Players;

    //still need the reference to the array so that we can convert it into a list.
    //public so that inspector can see the process,
    //set to private later because the inspector doesn't need to see.
    [Header("Character Teams")]
    private MoveableCharacter[] EnemyTeam;
    private MoveableCharacter[] PlayerTeam;

    [Header("Individual Characters")]
    public CharacterStats current;
    public MoveableCharacter returnUnit = null;

    //Variables that hold the random X and Y coordinates
    //We use this to generate a random tile on the map to select
    private int randNumX;
    private int randNumY;

    public void Initialize()
    {
        //Set empty lists
        Enemies = new List<MoveableCharacter>();
        Players = new List<MoveableCharacter>();
        //get a reference to the array 
        //Enemies
        EnemyTeam = SelectionManager.Instance.EnemyTeam.Team;
        //Players
        PlayerTeam = SelectionManager.Instance.PlayerTeam.Team;

        //looop through the indexes and add them to the list;
        //Enemies
        for (int i = 0; i < EnemyTeam.Length; i++)
        {
            Enemies.Add(EnemyTeam[i]);
        }
        //Players
        for (int i = 0; i < PlayerTeam.Length; i++)
        {
            Players.Add(PlayerTeam[i]);
        }

    }

    void Update()
    {

        if (GameManager.Instance.GameState != (int)GameManager.GameStates.Initialize && GameManager.Instance.GameState != (int)GameManager.GameStates.GameOver)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                //if an enemy is dead, remove them from the list
                if (!Enemies[i].m_isSelectable)
                {
                    Enemies.RemoveAt(i);
                }
            }
            //We first check if we are allowed to move (via gamestate)
            if (GameManager.Instance.GameState == (int)GameManager.GameStates.AIMove)
            {
                //check if there is at least 1 enemy in play on the field;
                if (Enemies.Count > 0)
                {
                    //Iterate through the entire team, generating random coordinates
                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        //Check if the unit at the current index can move
                        //check if the previous unit has moved, if they have then move this unit
                        if (i == 0)
                        {
                            if (!Enemies[i].m_moving && !Enemies[i].m_hasMoved)
                            {
                                ExecuteMove(Enemies[i]);
                            }
                        }

                        else if (Enemies[i - 1].m_hasMoved)
                        {
                            if (!Enemies[i].m_moving && !Enemies[i].m_hasMoved)
                            {
                                ExecuteMove(Enemies[i]);
                            }
                        }

                        if (i == Enemies.Count - 1 && Enemies[Enemies.Count - 1].m_hasMoved)
                        {
                            //TURN IS OVER
                            print("Turn over!");
                            GameManager.Instance.EndAITurn(Enemies);
                        }
                    }
                }
            }
        }
    }

    public void ExecuteMove(MoveableCharacter Enemy)
    {
        //FOR LATER
        //PseudoCode:
        //find the nearest player through a recursive function;
        //Move to that nearest player
        //attack that nearest player
        //end this enemies turn;

        //Check if there are still players to attack;
        if (Players.Count <= 0)
        {
            //if not,
            //Gameover! AI wins!
            //GameManager.Instance.DoGameOver("You lost!");
        }

        else
        {
            /*
            //if there are run the algorithm.
            //Set target for EnemyTeam.Team[i] to FindclosestTarget();
            //Move to (FindClosestTarget(Enemyteam.Team[i].m_currentPos;
            //Attaack target
            for (int i = 0; i < PlayerTeam.Length; i++)
            {
                //SET TARGET
                current = EnemyTeam[i].GetComponent<CharacterStats>();
                current.m_target = FindClosestTarget(EnemyTeam[i].m_CurrentLocation);

                //MOVE TO TARGET
                EnemyTeam[i].m_Destination = current.m_target.m_CurrentLocation;
                EnemyTeam[i].m_isSelected = true;
                EnemyTeam[i].m_moving = true;

                //ATTACK TARGET
                current.AttackTarget(current.m_target.GetComponent<CharacterStats>(), 0);
            }
            */

        }


        //FOR NOW
        //Generate a random X and Y
        //Since the map is hardcoded as a 9x15 Map we generate an X from 0 - 8, and a Y from 0-14
        //loop until a non occupied tile is found;
        do
        {
            randNumX = (int)Random.Range(0, 8);
            randNumY = (int)Random.Range(0, 14);
            print("RandomIndex");
        } while (Map.Instance.MAP[randNumX, randNumY].m_isOccupied || !Enemy.CheckMoveRange(new Vector2(randNumX, randNumY)));

        //Move the unit to Map[randNumX, randNumY]
        //Set the m_destination
        //Invoke the MoveToDestinationMethod by settting the destination, and m_isSelected + m_moving to true;
        Enemy.m_Destination = new Vector2(randNumX, randNumY);
        Enemy.m_isSelected = true;
        Enemy.m_moving = true;
    }

    private MoveableCharacter FindClosetsTarget(Vector2 StartLocation, MoveableCharacter currentUnit)
    {
        //count the steps to each player character;
        //return the smallest value of steps;
        returnUnit = null;
        Vector3 temp = Vector3.zero;
        float distance = 0, shortestDistance = 0;

        for (int i = 0; i < Players.Count; i++)
        {
            temp = Map.Instance.GetPosition(Players[i].m_CurrentLocation.x, Players[i].m_CurrentLocation.y) - Map.Instance.GetPosition(currentUnit.m_CurrentLocation.x, currentUnit.m_CurrentLocation.y);
            distance = temp.magnitude;

            if (shortestDistance == 0)
            {
                shortestDistance = distance;
                returnUnit = Players[i];
            }

            else if (distance < shortestDistance)
            {
                shortestDistance = distance;
                returnUnit = Players[i];
            }
        }



        return returnUnit;
    }

}