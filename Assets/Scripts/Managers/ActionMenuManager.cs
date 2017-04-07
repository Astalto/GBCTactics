using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionMenuManager : singleton<ActionMenuManager>
{
    public enum ActionChoice
    {
        Attack = 0,
        Abilities = 1,
        End_Turn = 2,
        Cancel = 3,
    };


    [Header("Action Menu")]
    public GameObject ActionMenu;

    [Header("AbilityMenu")]
    public GameObject AbilityMenu;

    [Header("Action Selection")]
    public Image[] Actions = new Image[4];
    public int ActionIndex;

    [Header("AbilitySelection")]
    public Image[] Abilities = new Image[3];
    public int AbilityIndex;

    [Header("Colors")]
    public Color DefaultColor;
    public Color HighlightColor;

    public Color DefaultAbilityColor;
    public Color HighlightAbilityColor;



    private void Start()
    {
        if (ActionMenu != null)
        {
            CloseActionMenu();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == (int)GameManager.GameStates.Action && !ActionMenu.activeInHierarchy)
        {
            OpenActionMenu();
        }
    }

    public void OpenActionMenu()
    {
        ActionMenu.SetActive(true);
    }

    public void CloseActionMenu()
    {
        DeselectAction(Actions[ActionIndex]);
        ActionIndex = 0;
        HighlightAction(Actions[ActionIndex]);
        ActionMenu.SetActive(false);
    }

    public void OpenAbilityMenu()
    {
        AbilityMenu.SetActive(true);
    }

    public void CloseAbilityMenu()
    {
        DeselectAction(Abilities[AbilityIndex]);
        ActionIndex = 0;
        HighlightAction(Abilities [AbilityIndex]);
        AbilityMenu.SetActive(false);
    }

    public void CycleNextAction()
    {
        DeselectAction(Actions[ActionIndex]);

        if (ActionIndex < Actions.Length - 1)
        {
            ActionIndex++;
        }

        else
        {
            ActionIndex = 0;
        }

        HighlightAction(Actions[ActionIndex]);
    }

    public void CyclePreviousAction()
    {
        DeselectAction(Actions[ActionIndex]);

        if (ActionIndex > 0)
        {
            ActionIndex--;
        }

        else
        {
            ActionIndex = Actions.Length - 1;
        }

        HighlightAction(Actions[ActionIndex]);
    }

    public void CycleNextAbility()
    {
        DeselectAction(Actions[AbilityIndex]);

        if (AbilityIndex < Actions.Length - 1)
        {
            AbilityIndex++;
        }

        else
        {
            AbilityIndex = 0;
        }

        HighlightAction(Actions[AbilityIndex]);
    }

    public void CyclePreviousAbility()
    {
        DeselectAction(Abilities[AbilityIndex]);

        if (ActionIndex > 0)
        {
            AbilityIndex--;
        }

        else
        {
            AbilityIndex = Abilities.Length - 1;
        }

        HighlightAction(Abilities[AbilityIndex]);
    }

    public void DeselectAction(Image i)
    {
        i.color = DefaultColor;
    }

    public void HighlightAction(Image i)
    {
        i.color = HighlightColor;
    }


    public void DeselectAbility(Image i)
    {
        i.color = DefaultAbilityColor;
    }

    public void HighlightAbility(Image i)
    {
        i.color = HighlightAbilityColor;
    }

}
