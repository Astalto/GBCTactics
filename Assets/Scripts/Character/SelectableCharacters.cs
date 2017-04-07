using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This scrip contains the functionality to allow the user to select his units.
/// As it is specifically for selecting players, the enemy team will not utilize this script.
/// </summary>

public class SelectableCharacters : MonoBehaviour
{
    [Header("Team Information")]
    public int DefaultTeamSize;
    public int TeamSize;

    public MoveableCharacter[] Team;
    public int m_selectionIndex;
    public MoveableCharacter SelectedCharacter;

    public bool m_isPlayerTeam;
    [SerializeField]
    private bool m_characterSelected;

    public bool IsPlayerTeam { get { return m_isPlayerTeam; } set { m_isPlayerTeam = value; } }
    public bool CharacterSelected { set { m_characterSelected = value; } }
    public int SelectionIndex { get { return m_selectionIndex; } set { m_selectionIndex = value; } }

    public void Initialize()
    {
        TeamSize = DefaultTeamSize;
        Team = new MoveableCharacter[DefaultTeamSize];

        if(!IsPlayerTeam)
        {
            for (int i = 0; i < TeamSize; i++)
            {
                Team[i] = GameObject.Find("Enemy" + i).GetComponent<MoveableCharacter>();
                Team[i].Initialize();
            }
        }

        else
        {
            for (int i = 0; i < TeamSize; i++)
            {
                Team[i] = GameObject.Find("PlayerMember" + i).GetComponent<MoveableCharacter>();
                Team[i].Initialize();
            }

            Map.Instance.SelectedTile = Team[0].m_SpawnCell;
        }


            for (int i = 0; i < TeamSize; i++)
            {
                if (i == 0)
                {
                    List<Ability> abilityRef = Team[i].GetComponent<CharacterStats>().Abilities;
                    abilityRef.Add(new Ability("Fire", 15, 15));
                    abilityRef.Add(new Ability("Ice Lance", 15, 25));
                    abilityRef.Add(new Ability("Gravity", 25, 50));
                }

                else if (i == 1)
                {
                    List<Ability> abilityRef = Team[i].GetComponent<CharacterStats>().Abilities;
                    abilityRef.Add(new Ability("Flaming Arrow", 15, 15));
                    abilityRef.Add(new Ability("Head Shot", 25, 30));
                    abilityRef.Add(new Ability("Piercing Shot", 20, 25));
                }

                else if (i == 2)
                {
                    List<Ability> abilityRef = Team[i].GetComponent<CharacterStats>().Abilities;
                    abilityRef.Add(new Ability("Lance Thrust", 15, 15));
                    abilityRef.Add(new Ability("Maim", 15, 25));
                    abilityRef.Add(new Ability("Odin's Fury", 20, 50));
                }

                else if (i == 3)
                {
                    List<Ability> abilityRef = Team[i].GetComponent<CharacterStats>().Abilities;
                    abilityRef.Add(new Ability("BackStab", 15, 25));
                    abilityRef.Add(new Ability("Juggulate", 25, 30));
                    abilityRef.Add(new Ability("Bomb Toss", 30, 50));
                }
            }
    }

    void Update()
    {
        if (GameManager.Instance.GameState != (int)GameManager.GameStates.Initialize && GameManager.Instance.GameState != (int)GameManager.GameStates.GameOver)
        {
            if (m_characterSelected)
            {
                //Toggle Team[selectionIndex].isSelected;

                if (IsPlayerTeam)
                {
                    Team[m_selectionIndex].m_isSelected = true;
                    //SelectionManager.Instance.log.AddEvent("TeamMember selected: " + Team[m_selectionIndex].gameObject.name);
                }

                if (!Team[m_selectionIndex].m_isSelectable)
                {
                    Team[m_selectionIndex].m_isSelected = false;
                    IncremenetSelectionIndex();
                    SetCurrentSelectedCharacter();
                    print("!selectable.....");
                }
            }

            else
            {
                SetCurrentSelectedCharacter();
            }

            RemoveInactiveCharacters();
        }
    }

    void SetCurrentSelectedCharacter()
    {
        SelectedCharacter = Team[m_selectionIndex];
    }

    public void IncremenetSelectionIndex()
    {
        if (SelectionIndex < TeamSize - 1)
        {
            SelectionIndex++;
        }

        else
        {
            SelectionIndex = 0;
        }
    }

    public void DecrementSelectionIndex()
    {
        if (SelectionIndex > 0)
        {
            SelectionIndex--;
        }

        else
        {
            SelectionIndex = TeamSize - 1;
        }
    }

    private void RemoveInactiveCharacters()
    {
        if (TeamSize > 0 && CheckForEnemies())
        {
            for (int i = 0; i < TeamSize; i++)
            {
                if (Team[i].m_hasMoved && Team[i].m_hasAttacked || Team[i].GetComponent<CharacterStats>().HP < 0)
                {
                    Team[i].GetComponent<SpriteRenderer>().color = Color.grey;
                    Team[i].m_isSelectable = false;
                    MoveableCharacter t = Team[i];
                    Team[i] = Team[TeamSize - 1];
                    Team[TeamSize - 1] = t;
                    TeamSize--;

                    //SelectionManager.Instance.log.AddEvent(SelectionManager.Instance.PlayerTeam.SelectedCharacter.name + "'s turn is complete.");

                    if (TeamSize <= 0)
                    {
                        //Round is over;
                        //reset player for next round;
                        //IF A UNIT IS DEAD DON"T RESET THEM//
                        for (int j = 0; j < 4; j++)
                        {
                            Team[j].m_isSelectable = true;
                            Team[j].m_hasMoved = false;
                            Team[j].m_hasAttacked = false;
                        }

                        TeamSize = 4;

                        if (m_isPlayerTeam)
                        {
                            GameManager.Instance.GameState = (int)GameManager.GameStates.AIMove;
                        }
                    }
                }
            }
        }

        else
        {
            //GameOver
            //GAME OVER
            print("YOU WIN");

            //set the gamestate to GameOver;
            GameManager.Instance.GameState = (int)GameManager.GameStates.GameOver;

            //Invoke the game over function
            GameManager.Instance.DoGameOver("You Win!");

        }

    }


    private bool CheckForEnemies()
    {
        //do a simple count check for enemies;
        if(SelectionManager.Instance.EnemyTeam.GetComponent<EnemyAI>().Enemies.Count > 0)
        {
            return true;
        }

        return false;
    }

    public void ResetTeam()
    {
        for(int i = 0; i < TeamSize; i++)
        {
            if (Team[i].m_isSelectable && Team[i].GetComponent<CharacterStats>().HP > 0)
            {
                Team[i].GetComponent<SpriteRenderer>().color = Color.white;
            }

            if(GameManager.Instance.GameState == (int)GameManager.GameStates.GameOver)
            {
                //reset the teamsize;
                TeamSize = DefaultTeamSize;

                //reset the spritecolors
                Team[i].GetComponent<SpriteRenderer>().color = Color.white;

                //if it's the gameoverstate either the game is just staring or a new round is starting;
                CharacterStats statsReference = Team[i].GetComponent<CharacterStats>();

                //if a character is disabled, re-enable it;
                if (!Team[i].gameObject.activeInHierarchy)
                {
                    Team[i].gameObject.SetActive(true);
                }

                //Reset all character booleans to their default states
                Team[i].m_isSelectable = true; //selectable true
                Team[i].m_hasMoved = Team[i].m_hasAttacked = Team[i].m_moving = Team[i].m_movingUp = Team[i].m_movingDown = Team[i].m_movingLeft = Team[i].m_movingRight = false; //the rest false
                
                //reset character's HP;
                statsReference.HP = statsReference.MaxHP;

                //reset character's AP;
                statsReference.AP = statsReference.MaxAP;

                //reset the selection indexes
                m_selectionIndex = 0;

                print("reset Member: " + i + " for new round!");

            }
        }
    }
}
