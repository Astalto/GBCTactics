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
        GetDefaultInput();

        if(GameManager.Instance.GameState == (int)GameManager.GameStates.Selecting)
        {
            GetSelectionInput();
        }

        else if(GameManager.Instance.GameState == (int)GameManager.GameStates.Moving)
        {
            GetMovementInput();
        }

        else if(GameManager.Instance.GameState == (int)GameManager.GameStates.Action)
        {
            GetActionInput();
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
                if (SelectableCharacters.Instance.SelectionIndex < SelectableCharacters.Instance.TeamSize - 1)
                {
                    SelectableCharacters.Instance.SelectionIndex++;
                }

                else
                {
                    SelectableCharacters.Instance.SelectionIndex = 0;
                }

                UpdateCursorLocation();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectableCharacters.Instance.CharacterSelected = true;

                //Set game state to moving;
                GameManager.Instance.GameState = (int)GameManager.GameStates.Moving;
            }
    }

    private void GetMovementInput()
    {

        //State should be Moving;
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Moving)
        {
            if (!SelectableCharacters.Instance.SelectedCharacter.m_moving)
            {
                if (Input.GetKeyDown(KeyCode.Return) && SelectableCharacters.Instance.SelectedCharacter.m_CurrentLocation != SelectableCharacters.Instance.SelectedCharacter.m_Destination)
                {
                    SelectableCharacters.Instance.SelectedCharacter.LightPath(SelectableCharacters.Instance.SelectedCharacter.m_CurrentLocation);
                }
            }
        }
    }

    private void GetActionInput()
    {

    }

    private void UpdateCursorLocation()
    {
        Map.Instance.DeselectTile();
        Map.Instance.SelectedTile = SelectableCharacters.Instance.Team[SelectableCharacters.Instance.SelectionIndex].m_CurrentLocation;
    }


}
