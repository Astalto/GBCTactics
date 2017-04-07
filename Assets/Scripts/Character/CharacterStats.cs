﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private int m_MoveRange;

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

    //could add crit, speed, weapon bonus, attack range

    public void Start()
    {
        EventLogger = SelectionManager.Instance.log;
        
    }

    public void AttackTarget(CharacterStats other, int attackType)
    {
        m_target = other.GetComponent<MoveableCharacter>();

        switch (attackType)
        {
            case 0:
                if (other.HP > 0 && TargetInRange() && attackType == 0)
                {
                    other.HP -= CalculateDamage(ATTPOW, other.DEF);

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

                break;
            case 1:
                if (other.HP > 0 && TargetInRange() && attackType == 0)
                {
                    other.HP -= CalculateDamage(ABPOW, other.MDEF);

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
                }
                break;
        }

        

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

    private bool TargetInRange()
    {
        Vector2 TargetLocIndex = m_target.m_CurrentLocation;
        Vector2 AttackerLocIndex = this.GetComponent<MoveableCharacter>().m_CurrentLocation;

        Vector2 TargetLocation = Map.Instance.GetPosition(TargetLocIndex.x, TargetLocIndex.y);
        Vector2 AttackerLocation = Map.Instance.GetPosition(AttackerLocIndex.x, AttackerLocIndex.y);

        if (TargetLocation.x == AttackerLocation.x)
        {
            if (TargetLocation.y < AttackerLocation.y + (blockDistance * this.RANGE) && TargetLocation.y > AttackerLocation.y)
            {
                //TOP
                //print("INRANGE TOP");
                return true;
            }

            else if (TargetLocation.y > AttackerLocation.y - (blockDistance * this.RANGE) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom
                //print("INRANGE BOTTOM");
                return true;
            }
        }

        else if (TargetLocation.y == AttackerLocation.y)
        {
            if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RANGE) && TargetLocation.x < AttackerLocation.x)
            {
                //left
                //print("INRANGE LEFT");
                return true;
            }

            else if (TargetLocation.x < AttackerLocation.x + (blockDistance * this.RANGE) && TargetLocation.x > AttackerLocation.x)
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
            if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RANGE) && TargetLocation.x < AttackerLocation.x
                && TargetLocation.y < AttackerLocation.y + (blockDistance * this.RANGE) && TargetLocation.y > AttackerLocation.y)
            {
                //If it is in range left and in range top, we know diagonally its up left
                //print("INRANGE TOP LEFT");
                return true;
            }

            else if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RANGE) && TargetLocation.x < AttackerLocation.x
                && TargetLocation.y > AttackerLocation.y - (blockDistance * this.RANGE) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom left
                //print("INRANGE BOTTOM LEFT");
                return true;
            }

            else if (TargetLocation.x < AttackerLocation.x + (blockDistance * this.RANGE) && TargetLocation.x > AttackerLocation.x
                && TargetLocation.y < AttackerLocation.y + (blockDistance * this.RANGE) && TargetLocation.y > AttackerLocation.y)
            {
                //Top right
                //print("INRANGE TOP RIGHT");
                return true;
            }

            else if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RANGE) && TargetLocation.x < AttackerLocation.x
                && TargetLocation.y > AttackerLocation.y - (blockDistance * this.RANGE) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom right
                //print("INRANGE BOTTOM RIGHT");
                return true;
            }
        }

        //print("NOT INRANGE");
        EventLogger.AddEvent(m_target.gameObject.name + " is not in range of " + this.gameObject.name);
        //EventLogger.AddEvent(this.gameObject.name + "'s turn is complete.");
        return false;
    }

}
