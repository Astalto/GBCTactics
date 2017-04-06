using UnityEngine;
using System.Collections;

/// <summary>
/// This scrip contains the functionality to allow the user to select his units.
/// As it is specifically for selecting players, the enemy team will not utilize this script.
/// </summary>

public class SelectableCharacters : MonoBehaviour
{
    [Header("Team Information")]
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
        Team = new MoveableCharacter[TeamSize];

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


    }

    void Update()
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
        if (TeamSize > 0)
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

                    SelectionManager.Instance.log.AddEvent(SelectionManager.Instance.PlayerTeam.SelectedCharacter.name + "'s turn is complete.");

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

                        GameManager.Instance.GameState = (int)GameManager.GameStates.AIMove;
                    }
                }
            }
        }

        else
        {
            if(!m_isPlayerTeam)
            {
                //player wins
                //enemy team is dead
            }

            else
            {
                //player lose
                //playerteam is dead
            }
        }

    }


    public void ResetTeam()
    {
        for(int i = 0; i < TeamSize; i++)
        {
            if (Team[i].m_isSelectable && Team[i].GetComponent<CharacterStats>().HP > 0)
            {
                Team[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
