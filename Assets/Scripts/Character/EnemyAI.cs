﻿using UnityEngine;
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
                                ExecuteMove(Enemies[i], i);
                            }
                        }

                        else if (Enemies[i - 1].m_hasMoved && Enemies[i - 1].m_hasAttacked)
                        {
                            if (!Enemies[i].m_moving && !Enemies[i].m_hasMoved)
                            {
                                ExecuteMove(Enemies[i], i);
                            }
                        }

                        if (Enemies[i].m_hasMoved && Enemies[i].GetComponent<CharacterStats>().m_target != null && !Enemies[i].m_hasAttacked)
                        {
                            DoAttack(Enemies[i]);
                            //print("Attacking " + Time.timeSinceLevelLoad);
                        }

                        else if (Enemies[i].m_hasMoved && !Enemies[i].m_hasAttacked)
                        {

                            Enemies[i].m_hasAttacked = true;
                            //print("End Turn " + Time.timeSinceLevelLoad);
                            //Enemies[i].m_moving = false;
                        }

                        if (i == Enemies.Count - 1 && Enemies[Enemies.Count - 1].m_hasMoved && Enemies[Enemies.Count - 1].m_hasAttacked)
                        {
                            //TURN IS OVER
                            //print("Turn over!");
                            GameManager.Instance.EndAITurn(Enemies);
                        }
                    }

                }
            }
        }
    }

    public void ExecuteMove(MoveableCharacter e, int index)
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
            //if there are run the algorithm.
            //Set target for EnemyTeam.Team[i] to FindclosestTarget();
            //Move to (FindClosestTarget(Enemyteam.Team[i].m_currentPos;
            //Attaack target
            current = e.GetComponent<CharacterStats>();
            current.m_target = FindClosestTarget(e.m_CurrentLocation, e.GetComponent<MoveableCharacter>());

            if(current.m_target == null)
            {
                GetRandomMove(e);
            }
                
            else
            {
                //MOVE TO TARGET
                Vector2 newDestination = new Vector2(current.m_target.m_CurrentLocation.x, current.m_target.m_CurrentLocation.y - current.ATTRANGE);
                if(Map.Instance.MAP[(int)newDestination.x, (int)newDestination.y].m_isOccupied)
                {
                    newDestination = new Vector2(current.m_target.m_CurrentLocation.x + 1, current.m_target.m_CurrentLocation.y - current.ATTRANGE);
                }
                e.m_Destination = newDestination;
                e.m_isSelected = true;
                e.m_moving = true;

            }

            //e.m_moving = false;
            //e.m_hasAttacked = true;
        }

    }

    void DoAttack(MoveableCharacter e)
    {
        current = e.GetComponent<CharacterStats>();

        // ATTACK TARGET
        //if has mana, use ability
        //else use reg attack
        current.AttackTarget(current.m_target.GetComponent<CharacterStats>(), 0);

        e.m_hasAttacked = true;
    }

    void GetRandomMove(MoveableCharacter Enemy)
    {
        //FOR NOW
        //Generate a random X and Y
        //Since the map is hardcoded as a 9x15 Map we generate an X from 0 - 8, and a Y from 0-14
        //loop until a non occupied tile is found;
        do
        {
            randNumX = (int)Random.Range(0, 8);
            randNumY = (int)Random.Range(0, 14);
            //print("RandomIndex");
        } while (Map.Instance.MAP[randNumX, randNumY].m_isOccupied || !Enemy.CheckMoveRange(new Vector2(randNumX, randNumY)));

        //Move the unit to Map[randNumX, randNumY]
        //Set the m_destination
        //Invoke the MoveToDestinationMethod by settting the destination, and m_isSelected + m_moving to true;
        Enemy.m_Destination = new Vector2(randNumX, randNumY);
        Enemy.m_isSelected = true;
        Enemy.m_moving = true;
    }

    private MoveableCharacter FindClosestTarget(Vector2 StartLocation, MoveableCharacter currentUnit)
    {
        //count the steps to each player character;
        //return the smallest value of steps;
        returnUnit = null;
        Vector3 temp = Vector3.zero;
        float distance = 0, shortestDistance = 0;

        for (int i = 0; i < Players.Count; i++)
        {
            temp = PlayerTeam[i].m_CurrentLocation - currentUnit.m_CurrentLocation;
            distance = temp.magnitude;
            //set the current units target to the current player to check;
            currentUnit.GetComponent<CharacterStats>().m_target = PlayerTeam[i];

            Vector2 check = new Vector2(current.m_target.m_CurrentLocation.x - current.ATTRANGE, current.m_target.m_CurrentLocation.y - current.ATTRANGE);

            if (current.GetComponent<MoveableCharacter>().CheckMoveRange(check))
            {
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
            //reset the current units target to null;
            currentUnit.GetComponent<CharacterStats>().m_target = null;
        }



        return returnUnit;
    }

}