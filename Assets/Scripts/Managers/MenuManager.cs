using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : singleton<MenuManager>
{
    public enum ActionChoice
    {
        Attack = 0,
        Item = 1,
        Spell = 2,
        Cancel = 3,
    };


    [Header("Action Menu")]
    public GameObject ActionMenu;
    public Image AttackAction;
    public Image ItemAction;
    public Image SpellAction;
    public Image CancelAction;

    [Header("Selection")]
    public Image[] Actions = new Image[4];
    public int ActionIndex;
    public Color DefaultColor;
    public Color HighlightColor;


    private void Start()
    {
        
        ActionMenu = GameObject.Find("ActionMenu");

        Actions[0] = AttackAction = GameObject.FindWithTag("Action_Attack").GetComponent<Image>();
        Actions[1] = ItemAction = GameObject.FindWithTag("Action_Item").GetComponent<Image>();
        Actions[2] = SpellAction = GameObject.FindWithTag("Action_Spell").GetComponent<Image>();
        Actions[3] = CancelAction = GameObject.FindWithTag("Action_Cancel").GetComponent<Image>();


        if (ActionMenu != null)
        {
            CloseActionMenu();
        }

    }

    private void Update()
    {
        if(GameManager.Instance.GameState == (int)GameManager.GameStates.Action && !ActionMenu.activeInHierarchy)
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
        DeselectAction();
        ActionIndex = 0;
        HighlightSelectedAction();
        ActionMenu.SetActive(false);
    }

    public void CycleNextAction()
    {
        DeselectAction();

        if(ActionIndex < Actions.Length - 1)
        {
            ActionIndex++;
        }

        else
        {
            ActionIndex = 0;
        }

        HighlightSelectedAction();
    }

    public void CyclePreviousAction()
    {
        DeselectAction();

        if (ActionIndex > 0)
        {
            ActionIndex--;
        }

        else
        {
            ActionIndex = Actions.Length - 1;
        }

        HighlightSelectedAction();
    }

    public void DeselectAction()
    {
        Actions[ActionIndex].color = DefaultColor;
    }

    public void HighlightSelectedAction()
    {
        Actions[ActionIndex].color = HighlightColor;
    }

}
