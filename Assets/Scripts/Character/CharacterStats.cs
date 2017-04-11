﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private int m_AttackPower;
    [SerializeField]
    private int m_AbilityPower;
    [SerializeField]
    private int m_AbilityPoints;
    [SerializeField]
    private int m_MaxAbilityPoints;
    [SerializeField]
    private int m_MaxHealthPoints;
    [SerializeField]
    private int m_HealthPoints;
    [SerializeField]
    private int m_DefensePower;
    [SerializeField]
    private int m_magicDefensePower;
    [SerializeField]
    private int m_AttackRange;
    [SerializeField]
    private int m_AbilityRange;
    [SerializeField]
    private int m_MoveRange;

    public Slider HealthBar;

    [Header("Damage Indication Information Station")]
    public Text DMGIndicator;
    public Color ResetColor;
    public float m_FadeSpeed;
    public bool m_Fading;

    public int blockDistance = 4;

    [Header("Target Info")]
    public MoveableCharacter m_target;

    [Header("Abilities")]
    public List<Ability> Abilities = new List<Ability>();

    //Reference to the eventLog to add strings to the log, for visual display of results to the player
    private EventLog EventLogger;

    public int ATTPOW { get { return m_AttackPower; } }
    public int ABPOW { get { return m_AbilityPower; } }
    public int MaxAP { get { return m_MaxAbilityPoints; } }
    public int AP { get { return m_AbilityPoints; } set { m_AbilityPoints = value; } }
    public int MaxHP { get { return m_MaxHealthPoints; } }
    public int HP { get { return m_HealthPoints; } set { m_HealthPoints = value; } }
    public int DEF { get { return m_DefensePower; } }
    public int MDEF { get { return m_magicDefensePower; } }
    public int RANGE { get { return m_AttackRange; } }
    public int MOVERANGE { get { return m_MoveRange; } }

    //could add crit, speed, weapon bonus

    public void Start()
    {
        ResetColor = DMGIndicator.color;
        DMGIndicator.gameObject.SetActive(false);
        EventLogger = SelectionManager.Instance.log;
        HealthBar.maxValue = MaxHP;
        m_Fading = false;
    }

    public void Update()
    {
        HealthBar.value = m_HealthPoints;

        if(m_Fading)
        {
            FadeText();

            if(DMGIndicator.color.a <= 0)
            {
                m_Fading = false;
                DMGIndicator.color = ResetColor;
                DMGIndicator.gameObject.SetActive(false);
            }
        }
    }

    public void AttackTarget(CharacterStats other, int attackType)
    {
        m_target = other.GetComponent<MoveableCharacter>();
        CharacterStats target_stats;

        switch (attackType)
        {
            case 0:
                if (other.HP > 0 && TargetInRange(attackType, Vector2.zero))
                {
                    print("Attacking with weapon");
                    other.HP -= CalculateDamage(ATTPOW, other.DEF);

                    //SET DMG TEXT TO CalculateDamage(ATTPOW, other.DEF);
                    target_stats = other.GetComponent<CharacterStats>();
                    target_stats.DMGIndicator.text = "" + CalculateDamage(ATTPOW, other.DEF);
                    target_stats.DMGIndicator.gameObject.SetActive(true);
                    target_stats.m_Fading = true;

                    EventLogger.AddEvent(this.gameObject.name + " hit " + other.gameObject.name + " for " + CalculateDamage(ATTPOW, other.DEF) + " damage.");
                    //animate taking damage && attacking;

                    m_target.m_isSelected = false;
                    GetComponent<MoveableCharacter>().m_isSelected = false;

                    if (other.HP <= 0)
                    {
                        EventLogger.AddEvent(this.gameObject.name + " killed: " + other.gameObject.name);
                        other.Kill();
                    }
                    else
                    {
                        EventLogger.AddEvent(other.gameObject.name + " has " + other.HP + " HP remaining!");
                    }
                }

                else
                {

                }

                break;
            case 1:
                if (other.HP > 0 && TargetInRange(attackType, Vector2.zero))
                {
                    print("Attacking with ability");
                    //if this has enough ap, do the attack; otherwise let the user know that they don't have enough and go back to ability selection state;
                    m_AbilityPower = Abilities[ActionMenuManager.Instance.AbilityIndex].Damage;

                    other.HP -= CalculateDamage(ABPOW, other.MDEF);

                    //SET DMG TEXT TO CalculateDamage(ATTPOW, other.DEF);
                    target_stats = other.GetComponent<CharacterStats>();
                    target_stats.DMGIndicator.text = "" + CalculateDamage(ABPOW, other.MDEF);
                    target_stats.DMGIndicator.gameObject.SetActive(true);
                    target_stats.m_Fading = true;

                    EventLogger.AddEvent(this.gameObject.name + " hit " + other.gameObject.name + " for " + CalculateDamage(ABPOW, other.MDEF) + " damage.");
                    //animate taking damage && attacking;

                    m_target.m_isSelected = false;
                    GetComponent<MoveableCharacter>().m_isSelected = false;

                    if (other.HP <= 0)
                    {
                        EventLogger.AddEvent(this.gameObject.name + " killed: " + other.gameObject.name);
                        other.Kill();
                    }
                    else
                    {
                        EventLogger.AddEvent(other.gameObject.name + " has " + other.HP + " HP remaining!");
                    }

                    m_AbilityPoints -= Abilities[ActionMenuManager.Instance.AbilityIndex].Cost;
                }
                break;
        }

        

    }

    public void FadeText()
    {
        Color tempColor = DMGIndicator.color;
        tempColor.a -= m_FadeSpeed * Time.deltaTime;
        DMGIndicator.color = tempColor;
    }

    public void Kill()
    {
        //Play death animation if necessary
        //remove character from field;
        this.GetComponent<MoveableCharacter>().m_isSelectable = false;
        this.GetComponent<MoveableCharacter>().m_isSelected = false;
        this.gameObject.SetActive(false);
    }

    private int CalculateDamage(int Attack, int EnemyDefense)
    {
        return Attack - EnemyDefense;
    }

    public bool TargetInRange(int attackType, Vector2 index)
    {

        int RangeToCheck;
        Vector2 TargetLocIndex = Vector2.zero;
        Vector2 AttackerLocIndex = Vector2.zero;

        Vector2 TargetLocation = Vector2.zero;
        Vector2 AttackerLocation = Vector2.zero;

        if (attackType == 0)
        {
            RangeToCheck = m_AttackRange;
        }

        else if(attackType == 1)
        {
            RangeToCheck = m_AbilityRange;
        }

        else
        {
            RangeToCheck = GetComponent<MoveableCharacter>().m_moveRange;

            TargetLocIndex = index;
            AttackerLocIndex = GetComponent<MoveableCharacter>().m_CurrentLocation;

            TargetLocation = Map.Instance.GetPosition(TargetLocation.x, TargetLocation.y);
            AttackerLocation = Map.Instance.GetPosition(AttackerLocation.x, AttackerLocation.y);
        }

        if (attackType != 2)
        {
            TargetLocIndex = m_target.m_CurrentLocation;
            AttackerLocIndex = this.GetComponent<MoveableCharacter>().m_CurrentLocation;

            TargetLocation = Map.Instance.GetPosition(TargetLocIndex.x, TargetLocIndex.y);
            AttackerLocation = Map.Instance.GetPosition(AttackerLocIndex.x, AttackerLocIndex.y);
        }

        if (TargetLocation.x == AttackerLocation.x)
        {
            if (TargetLocation.y < AttackerLocation.y + (blockDistance * RangeToCheck) && TargetLocation.y > AttackerLocation.y)
            {
                //TOP
                //print("INRANGE TOP");
                return true;
            }

            else if (TargetLocation.y > AttackerLocation.y - (blockDistance * RangeToCheck) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom
                //print("INRANGE BOTTOM");
                return true;
            }
        }

        else if (TargetLocation.y == AttackerLocation.y)
        {
            if (TargetLocation.x > AttackerLocation.x - (blockDistance * RangeToCheck) && TargetLocation.x < AttackerLocation.x)
            {
                //left
                //print("INRANGE LEFT");
                return true;
            }

            else if (TargetLocation.x < AttackerLocation.x + (blockDistance * RangeToCheck) && TargetLocation.x > AttackerLocation.x)
            {
                //right
                //print("INRANGE RIGHT");
                return true;
            }
        }

        //If the Target's X AND Y are not equal to the attacker one of two possibilities:
        //(1)  Target is not in range
        //(2)  Target is not aligned in the cardinal direction, possibly in a diagonal to the attacker
        //If the second case is true, its also true the Target X and Y will be different from the attacker
        //Hence why if the X AND Y don't match, we still none the less check if the target is in range horizontally AND vertically
        else if (TargetLocation.x != AttackerLocation.x && TargetLocation.y != AttackerLocation.y)
        {
            if (TargetLocation.x > AttackerLocation.x - (blockDistance * RangeToCheck) && TargetLocation.x < AttackerLocation.x
                && TargetLocation.y < AttackerLocation.y + (blockDistance * RangeToCheck) && TargetLocation.y > AttackerLocation.y)
            {
                //If it is in range left and in range top, we know diagonally its up left
                //print("INRANGE TOP LEFT");
                return true;
            }

            else if (TargetLocation.x > AttackerLocation.x - (blockDistance * RangeToCheck) && TargetLocation.x < AttackerLocation.x
                && TargetLocation.y > AttackerLocation.y - (blockDistance * RangeToCheck) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom left
                //print("INRANGE BOTTOM LEFT");
                return true;
            }

            else if (TargetLocation.x < AttackerLocation.x + (blockDistance * RangeToCheck) && TargetLocation.x > AttackerLocation.x
                && TargetLocation.y < AttackerLocation.y + (blockDistance * RangeToCheck) && TargetLocation.y > AttackerLocation.y)
            {
                //Top right
                //print("INRANGE TOP RIGHT");
                return true;
            }

            else if (TargetLocation.x > AttackerLocation.x - (blockDistance * RangeToCheck) && TargetLocation.x < AttackerLocation.x
                && TargetLocation.y > AttackerLocation.y - (blockDistance * RangeToCheck) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom right
                //print("INRANGE BOTTOM RIGHT");
                return true;
            }
        }


        if (attackType != 2)
        {
            //print("NOT INRANGE");
            EventLogger.AddEvent(m_target.gameObject.name + " is not in range of " + this.gameObject.name);
            //EventLogger.AddEvent(this.gameObject.name + "'s turn is complete.");
            m_target.m_isSelected = false;
            GetComponent<MoveableCharacter>().m_isSelected = false;
        }

        else
        {
            if (Map.Instance.m_currentSelection.x == AttackerLocIndex.x)
            {
                if (Map.Instance.m_currentSelection.y == AttackerLocIndex.y)
                {
                    print(true);
                    return true;
                }
            }

        }

        print(index + " " + TargetLocIndex);
        print(false);
        return false;
    }

}
