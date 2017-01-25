using UnityEngine;
using System.Collections;

public class SelectableCharacters : singleton<SelectableCharacters>
{
    public int TeamSize;

    public MoveableCharacter[] Team;
    public int m_selectionIndex;
    public MoveableCharacter SelectedCharacter;

    private bool m_characterSelected;

    public bool CharacterSelected { set { m_characterSelected = value; } }
    public int SelectionIndex { get { return m_selectionIndex; } set { m_selectionIndex = value; } }

    void Start()
    {
        Team = new MoveableCharacter[TeamSize];

        for (int i = 0; i < TeamSize; i++)
        {
            Team[i] = GameObject.Find("PlayerMember" + i).GetComponent<MoveableCharacter>();
        }
    }

    void Update()
    {
        if(m_characterSelected)
        {
            //Toggle Team[selectionIndex].isSelected;
            Team[m_selectionIndex].m_isSelected = true;
            print("TeamMember Selected " + Team[m_selectionIndex].name);
        }

        else
        {
            SetCurrentSelectedCharacter();
        }
    }

    void SetCurrentSelectedCharacter()
    {
        SelectedCharacter = Team[m_selectionIndex];
    }
}
