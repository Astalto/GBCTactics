using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used to get player input in the various stages (GameStates) of the game.
/// The reason for this is that when using a controller, the selection button will be the same for
/// multiple things.
/// </summary>

public class InputManager : singleton<InputManager>
{
    private bool m_selectingUnit, m_selectingTile, m_selectingAction;

    private void Update()
    {


        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Selecting)
        {
            GetDefaultInput();
            GetSelectionInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Moving)
        {
            GetDefaultInput();
            GetMovementInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Action)
        {
            GetActionInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Attacking)
        {
            GetEnemySelectionInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.AIMove)
        {
            GetDefaultInput();
        }

    }

    private void GetDefaultInput()
    {
        //Get a reference to the current selcted tile in the map
        Vector2 currentTile = Map.Instance.SelectedTile;

        //Adjust the x or y value based on player input.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Set Current Cell to deselected;
            //set x-- cell to selected; above
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.SelectedTile.x - 1 >= 0)
            {
                currentTile.x--;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Deselect
            //Select x++ below
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.m_currentSelection.x + 1 < Map.Instance.Rows)
            {
                currentTile.x++;
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //y++ right
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.m_currentSelection.y + 1 < Map.Instance.Columns)
            {
                currentTile.y++;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //y-- left
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.m_currentSelection.y - 1 >= 0)
            {
                currentTile.y--;
            }
        }

        //set the selected tile from the Map script to the new value;
        Map.Instance.SelectedTile = currentTile;

        //Set the that selected tile to be selected;
        Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = true;

    }

    private void GetSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectionManager.Instance.SelectNextUnit(SelectionManager.Instance.PlayerTeam);

            UpdateCursorLocation(SelectionManager.Instance.PlayerTeam);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectableCharacters pChars = SelectionManager.Instance.PlayerTeam;
            pChars.CharacterSelected = true;

            if (!pChars.SelectedCharacter.m_hasMoved)
            {
                //Set game state to moving;
                GameManager.Instance.GameState = (int)GameManager.GameStates.Moving;
            }

            else if (!pChars.SelectedCharacter.m_hasAttacked)
            {
                //Set game state to Action;
                GameManager.Instance.GameState = (int)GameManager.GameStates.Action;
            }

            else
            {
                pChars.CharacterSelected = false;
            }

        }
    }

    private void GetEnemySelectionInput()
    {
        //UpdateEnemyCursorLocation();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectionManager.Instance.SelectNextUnit(SelectionManager.Instance.EnemyTeam);

            UpdateCursorLocation(SelectionManager.Instance.EnemyTeam);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectableCharacters eChars = SelectionManager.Instance.EnemyTeam;
            SelectableCharacters pChars = SelectionManager.Instance.PlayerTeam;

            CharacterStats target = eChars.Team[eChars.SelectionIndex].GetComponent<CharacterStats>();
            CharacterStats attacker = pChars.Team[pChars.SelectionIndex].GetComponent<CharacterStats>();

            SelectionManager.Instance.log.AddEvent(attacker.gameObject.name + " attacking: " + target.gameObject.name);

            eChars.Team[eChars.SelectionIndex].m_isSelected = true;

            //Attack enemy
            attacker.AttackTarget(target);

            pChars.Team[pChars.SelectionIndex].m_hasAttacked = true;

            GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;

            UpdateCursorLocation(pChars);
        }
    }

    private void GetMovementInput()
    {

        //State should be Moving;
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Moving)
        {
            MoveableCharacter s = SelectionManager.Instance.PlayerTeam.SelectedCharacter;

            if (!s.m_moving)
            {
                if (Input.GetKeyDown(KeyCode.Return) && s.m_CurrentLocation != s.m_Destination)
                {
                    s.LightPath(s.m_CurrentLocation);
                    //enable action select; (BEFORE MOVEMENT)
                    //GameManager.Instance.GameState = (int)GameManager.GameStates.Action;
                }
            }
        }
    }

    private void GetActionInput()
    {
        //State should me Action
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Action)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //Move up untill at the top of the menu(i.e. highlight a different option
                MenuManager.Instance.CyclePreviousAction();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Move down untill the same as top.
                MenuManager.Instance.CycleNextAction();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                //Enter the menu that has been chosen,
                //if attack, run the attack code, and change state to Animating
                //else if items, display possible items. (seperate menu)
                //else if spells, display possible spells. (Seperate menu)
                //else if cancel, close menu

                if (MenuManager.Instance.ActionIndex == (int)MenuManager.ActionChoice.Attack)
                {
                    //print("Action chosen: attack");

                    GameManager.Instance.GameState = (int)GameManager.GameStates.Attacking;
                }

                else if (MenuManager.Instance.ActionIndex == (int)MenuManager.ActionChoice.Cancel)
                {
                    //print("Action chosen: cancel");

                    //Deselect character
                    SelectionManager.Instance.DeSelectCharacter(SelectionManager.Instance.PlayerTeam);

                    //Set game state to selecting
                    GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;
                }

                //Close the UI menu
                MenuManager.Instance.CloseActionMenu();
            }
        }
    }

    private void UpdateCursorLocation(SelectableCharacters s)
    {
        MoveableCharacter unit = s.Team[s.SelectionIndex];

        Map.Instance.DeselectTile();
        Map.Instance.SelectedTile = unit.m_CurrentLocation;
    }



}
