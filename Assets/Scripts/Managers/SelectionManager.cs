using UnityEngine;
using System.Collections;

public class SelectionManager : singleton<SelectionManager>
{

    [Header("SelectableCharacterScripts")]
    public SelectableCharacters PlayerTeam;
    public SelectableCharacters EnemyTeam;

    public EventLog log;

    private void OnEnable()
    {
        PlayerTeam = GameObject.Find("PlayerTeam").GetComponent<SelectableCharacters>();
        EnemyTeam = GameObject.Find("EnemyTeam").GetComponent<SelectableCharacters>();

        PlayerTeam.IsPlayerTeam = true;
        EnemyTeam.IsPlayerTeam = false;

        log = GameObject.FindGameObjectWithTag("EventLog").GetComponent<EventLog>();
    }

    public void Iniitialize()
    {
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Initialize)
        {
            //if it is the first time running th game,
            //Initialize them first;
            EnemyTeam.Initialize();
            PlayerTeam.Initialize();

            EnemyTeam.ResetTeam();
            PlayerTeam.ResetTeam();

            return;
        }

        //otherwise, reset the teams first;
        EnemyTeam.ResetTeam();
        PlayerTeam.ResetTeam();

        EnemyTeam.Initialize();
        PlayerTeam.Initialize();

    }

    public void SelectNextUnit(SelectableCharacters Team)
    {
        Team.IncremenetSelectionIndex();
    }

    public void SelectPreviousUnit(SelectableCharacters Team)
    {
        Team.DecrementSelectionIndex();
    }

    public void SelectCharacter(SelectableCharacters Team)
    {
        Team.SelectedCharacter.m_isSelected = true;
        Team.CharacterSelected = true;
    }

    public void DeSelectCharacter(SelectableCharacters Team)
    {
        Team.SelectedCharacter.m_isSelected = false;
        Team.CharacterSelected = false;
    }

}
