using UnityEngine;
using System.Collections;

/// <summary>
/// This script contains the functionality to allow the user to select his units.
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
    private bool m_characterSelected;

    public bool IsPlayerTeam { get { return m_isPlayerTeam; }  set { m_isPlayerTeam = value; } }
    public bool CharacterSelected { set { m_characterSelected = value; } }
    public int SelectionIndex { get { return m_selectionIndex; } set { m_selectionIndex = value; } }

    void Start()
    {
        Team = new MoveableCharacter[TeamSize];

        if (IsPlayerTeam)
        {
            for (int i = 0; i < TeamSize; i++)
            {
                Team[i] = GameObject.Find("PlayerMember" + i).GetComponent<MoveableCharacter>();
            }
        }

        else
        {
            for (int i = 0; i < TeamSize; i++)
            {
                Team[i] = GameObject.Find("Enemy" + i).GetComponent<MoveableCharacter>();
            }
        }
    }

    void Update()
    {
        if(m_characterSelected)
        {
            //Toggle Team[selectionIndex].isSelected;
            
            Team[m_selectionIndex].m_isSelected = true;
            //print("TeamMember Selected " + Team[m_selectionIndex].name);

            if (!Team[m_selectionIndex].m_isSelectable)
            {
                Team[m_selectionIndex].m_isSelected = false;
                IncremenetSelectionIndex();
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
        if(SelectionIndex > 0)
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
        for(int i = 0; i < TeamSize; i++)
        {
            if(Team[i].m_hasMoved && Team[i].m_hasAttacked)
            {
                Team[i].m_isSelectable = false;
                MoveableCharacter t = Team[i];
                Team[i] = Team[TeamSize - 1];
                Team[TeamSize - 1] = t;
                TeamSize--;

                if(TeamSize <= 0)
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

}
