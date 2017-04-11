using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is used to get player input in the various stages (GameStates) of the game.
/// The reason for this is that when using a controller, the selection button will be the same for
/// multiple things.
/// </summary>

public class InputManager : singleton<InputManager>
{
    private bool m_selectingUnit, m_selectingTile, m_selectingAction;
    private Text m_stateText;

    public Text StateText { get { return m_stateText; } }

    private void OnEnable()
    {
        m_stateText = GameObject.FindGameObjectWithTag("StateNotifier").GetComponent<Text>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Selecting)
        {
            m_stateText.text = "Selecting a unit";
            GetSelectionInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Moving)
        {
            m_stateText.text = "Moving a unit";
            GetMapInput();
            GetMovementInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Action)
        {
            m_stateText.text = "Pending unit action";
            GetActionInput();
        }

        else if(GameManager.Instance.GameState == (int)GameManager.GameStates.Ability)
        {
            m_stateText.text = "Selecting ability";
            GetAbilityInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Attacking)
        {
            m_stateText.text = "Selecting a target.";
            GetEnemySelectionInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.Cast)
        {
            m_stateText.text = "Selecting a target.";
            GetEnemySelectionInput();
        }

        else if (GameManager.Instance.GameState == (int)GameManager.GameStates.AIMove)
        {
            m_stateText.text = "AI turn";
            //StartCoroutine(SimulateAI());
        }

        else if(GameManager.Instance.GameState == (int)GameManager.GameStates.GameOver)
        {
            m_stateText.text = "Game complete!";
        }

    }

    private void GetMapInput()
    {
        //Get a reference to the current selcted tile in the map
        //print(Map.Instance.SelectedTile);
        Vector2 currentTile = Map.Instance.SelectedTile;

        //Adjust the x or y value based on player input.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Set Current Cell to deselected;
            //set x-- cell to selected; above
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.SelectedTile.x - 1 >= 0)
                //&& SelectionManager.Instance.PlayerTeam.SelectedCharacter.CheckMoveRange(new Vector2(Map.Instance.SelectedTile.x - 1, Map.Instance.SelectedTile.y)))
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
                //&& SelectionManager.Instance.PlayerTeam.SelectedCharacter.CheckMoveRange(new Vector2(Map.Instance.SelectedTile.x + 1, Map.Instance.SelectedTile.y)))
            {
                currentTile.x++;
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //y++ right
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.m_currentSelection.y + 1 < Map.Instance.Columns)
                //&& SelectionManager.Instance.PlayerTeam.SelectedCharacter.CheckMoveRange(new Vector2(Map.Instance.SelectedTile.x, Map.Instance.SelectedTile.y + 1)))
            {
                currentTile.y++;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //y-- left
            Map.Instance.MAP[(int)currentTile.x, (int)currentTile.y].m_isSelected = false;

            if (Map.Instance.m_currentSelection.y - 1 >= 0)
                //&& SelectionManager.Instance.PlayerTeam.SelectedCharacter.CheckMoveRange(new Vector2(Map.Instance.SelectedTile.x, Map.Instance.SelectedTile.y - 1)))
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
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            SelectionManager.Instance.SelectPreviousUnit(SelectionManager.Instance.PlayerTeam);

            UpdateCursorLocation(SelectionManager.Instance.PlayerTeam);
        }

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
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            SelectionManager.Instance.SelectPreviousUnit(SelectionManager.Instance.EnemyTeam);

            UpdateCursorLocation(SelectionManager.Instance.EnemyTeam);
        }

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

            if (ActionMenuManager.Instance.AbilityMenu.activeInHierarchy)
            {
                //Attack enemy with ability dmg
                attacker.AttackTarget(target, 1);
            }

            else
            {
                //Attack enemy with weapon dmg
                attacker.AttackTarget(target, 0);
            }

            pChars.Team[pChars.SelectionIndex].m_hasAttacked = true;

            GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;

            UpdateCursorLocation(pChars);

            ActionMenuManager.Instance.CloseAbilityMenu();
            ActionMenuManager.Instance.CloseActionMenu();

        }
    }

    private void GetMovementInput()
    {

        //State should be Moving;
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Moving)
        {
            MoveableCharacter selectedCharacter = SelectionManager.Instance.PlayerTeam.SelectedCharacter;

            if (!selectedCharacter.m_moving)
            {
                if (Input.GetKeyDown(KeyCode.Return) && selectedCharacter.m_CurrentLocation != selectedCharacter.m_Destination)
                {
                    if(!Map.Instance.MAP[(int)selectedCharacter.m_Destination.x, (int)selectedCharacter.m_Destination.y].m_isOccupied)
                    {
                        selectedCharacter.LightPath(selectedCharacter.m_CurrentLocation);
                    }

                    else
                    {
                        SelectionManager.Instance.log.AddEvent("Tile occupied, please select a diferent tile!");
                    }

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
                ActionMenuManager.Instance.CyclePreviousAction();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Move down untill the same as top.
                ActionMenuManager.Instance.CycleNextAction();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                //Enter the menu that has been chosen,
                //if attack, run the attack code, and change state to Animating
                //else if abilites, display possible abilites. (Seperate menu)
                //else if end_turn, end the characters turn. (seperate menu)
                //else if cancel, close menu

                if (ActionMenuManager.Instance.ActionIndex == (int)ActionMenuManager.ActionChoice.Attack)
                {
                    //print("Action chosen: attack");
                    //SelectionManager.Instance.PlayerTeam.SelectedCharacter.GetComponent<AnimationController>().m_animator.SetTrigger("Attacking");

                    GameManager.Instance.GameState = (int)GameManager.GameStates.Attacking;

                    Vector2 index;
                    //highlight current selected enemy character;
                    index = SelectionManager.Instance.EnemyTeam.SelectedCharacter.m_CurrentLocation;
                    Map.Instance.m_currentSelection = index;

                    if (!Map.Instance.MAP[(int)index.x, (int)index.y].m_isOccupied)
                    {
                        do
                        {
                            SelectionManager.Instance.EnemyTeam.IncremenetSelectionIndex();
                            index = SelectionManager.Instance.EnemyTeam.SelectedCharacter.m_CurrentLocation;
                            Map.Instance.m_currentSelection = index;

                        } while (Map.Instance.MAP[(int)index.x, (int)index.y].m_isOccupied);
                    }

                }

                else if (ActionMenuManager.Instance.ActionIndex == (int)ActionMenuManager.ActionChoice.Abilities)
                {
                    ActionMenuManager.Instance.SetAbilityText(SelectionManager.Instance.PlayerTeam.SelectedCharacter.GetComponent<CharacterStats>().Abilities);

                    ActionMenuManager.Instance.OpenAbilityMenu();

                    GameManager.Instance.GameState = (int)GameManager.GameStates.Ability;

                }

                else if (ActionMenuManager.Instance.ActionIndex == (int)ActionMenuManager.ActionChoice.End_Turn)
                {
                    //set the current playerchar's has_attacked to true;
                    SelectionManager.Instance.PlayerTeam.SelectedCharacter.m_hasAttacked = true;

                    //set the gamestate back to selecting;
                    GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;


                }

                else if (ActionMenuManager.Instance.ActionIndex == (int)ActionMenuManager.ActionChoice.Cancel)
                {
                    //print("Action chosen: cancel");

                    //Set game state to selecting
                    GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;
                }

                //Deselect character
                SelectionManager.Instance.DeSelectCharacter(SelectionManager.Instance.PlayerTeam);

                //Close the UI menu
                if (GameManager.Instance.GameState != (int)GameManager.GameStates.Ability)
                {
                    ActionMenuManager.Instance.CloseActionMenu();
                }
            }
        }
    }

    public void GetAbilityInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Move up untill at the top of the menu(i.e. highlight a different option
            ActionMenuManager.Instance.CyclePreviousAbility();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Move down untill the same as top.
            ActionMenuManager.Instance.CycleNextAbility();
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {

            Vector2 index;
            //highlight current selected enemy character;
            index = SelectionManager.Instance.EnemyTeam.SelectedCharacter.m_CurrentLocation;
            Map.Instance.m_currentSelection = index;

            if (!Map.Instance.MAP[(int)index.x, (int)index.y].m_isOccupied)
            {
                do
                {
                    SelectionManager.Instance.EnemyTeam.IncremenetSelectionIndex();
                    index = SelectionManager.Instance.EnemyTeam.SelectedCharacter.m_CurrentLocation;
                    Map.Instance.m_currentSelection = index;

                } while (Map.Instance.MAP[(int)index.x, (int)index.y].m_isOccupied);
            }

            GameManager.Instance.GameState = (int)GameManager.GameStates.Cast;
        }
    }

    private void UpdateCursorLocation(SelectableCharacters s)
    {
        MoveableCharacter unit = s.Team[s.SelectionIndex];

        Map.Instance.DeselectTile();
        Map.Instance.SelectedTile = unit.m_CurrentLocation;
    }

}
