using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{

    //Reference to the EnemyCharacters class since it holds an array storing the AI characters
    public List<MoveableCharacter> Enemies;
    public MoveableCharacter[] EnemyTeam;

    //Variables that hold the random X and Y coordinates
    //We use this to generate a random tile on the map to select
    private int randNumX;
    private int randNumY;

    void Start()
    {
        EnemyTeam = SelectionManager.Instance.EnemyTeam.Team; 

        for (int i = 0; i < EnemyTeam.Length; i++)
        {
            Enemies.Add(EnemyTeam[i]);
        }
    }

    void Update()
    {
        //We first check if we are allowed to move
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.AIMove)
        {
            //Iterate through the entire team, generating random coordinates
            for (int i = 0; i < Enemies.Count; i++)
            {
                //if an enemy is dead, remove them from the list
                if(!Enemies[i].m_isSelectable)
                {
                    Enemies.RemoveAt(i);
                }

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

    public void ExecuteMove(MoveableCharacter Enemy)
    {
        //Generate a random X and Y
        //Since the map is hardcoded as a 9x15 Map we generate an X from 0 - 8, and a Y from 0-14
        //loop until a non occupied tile is found;
        do
        {
            randNumX = (int)Random.Range(0, 8);
            randNumY = (int)Random.Range(0, 14);
        } while (Map.Instance.MAP[randNumX, randNumY].m_isOccupied);

        //Move the unit to Map[randNumX, randNumY]
        //Set the m_destination
        //Invoke the MoveToDestinationMethod
        Enemy.m_Destination = new Vector2(randNumX, randNumY);
        Enemy.m_isSelected = true;
        Enemy.m_moving = true;
    }
}
