using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    //Reference to the EnemyCharacters class since it holds an array storing the AI characters
    public SelectableCharacters aiList;

    //Boolean to hold whether the AI's turn has started
    //Left public so that the code overseeing the player's turn will manually set this to true once all player characters have moved
    //This indicates to the AI that it can start
    public bool aiTurn; //CAN determine if it's AI"S turn by checking the GAMESTATE

    //Variables that hold the random X and Y coordinates
    //We use this to generate a random tile on the map to select
    private int randNumX;
    private int randNumY;

	void Start()
    {
        //Set to false on startup, the player will always go first
        aiTurn = false;
        aiList = SelectionManager.Instance.EnemyTeam;
    }
	
	void Update ()
    {
        //We first check if we are allowed to move
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.AIMove)
        {
            //Iterate through the entire team, generating random coordinates
            for (int i = 0; i < aiList.TeamSize; i++)
            {
                //if(!aiList.Team[i].gameObject.activeInHierarchy)
                //{
                //    i++;
                //}
                //First check if the unit at the current index can move
                if (!aiList.Team[i].m_hasMoved)
                {
                    //Generate a random X and Y
                    //Since the map is hardcoded as a 15x9 Map we generate an X from 0-14, and a y from 0 - 8
                    randNumX = (int)Random.Range(0, 8);
                    randNumY = (int)Random.Range(0, 14);

                    //Move the unit to Map[randNumX, randNumY]
                    //Get a reference to the moveable character script so we can
                    //Set the m_destination
                    //Invoke the MoveToDestinationMethod
                    aiList.Team[i].GetComponent<MoveableCharacter>().m_Destination = new Vector2(randNumX, randNumY);
                    aiList.Team[i].GetComponent<MoveableCharacter>().m_isSelected = true;
                    aiList.Team[i].GetComponent<MoveableCharacter>().m_moving = true;
                }

                if (i == aiList.TeamSize - 1 && aiList.Team[i].m_hasMoved)
                {
                    //TURN IS OVER
                    GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;
                }
            }
            //Once that is done, we set aiTurn = false
            aiTurn = false;
        }
	}
}
