using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public int m_AttackPower;
    public int m_HealthPoints;
    public int m_DefensePower;
    public int m_AttackRange;

    public int blockDistance = 4;

    [Header("Target Info")]
    public MoveableCharacter m_target;

    //Reference to the eventLog to add strings to the log, for visual display of results to the player
    private EventLog logOfEvents;

    public int AP { get { return m_AttackPower; } }
    public int HP { get { return m_HealthPoints; } set { m_HealthPoints = value; } }
    public int DEF { get { return m_DefensePower; } }
    public int RNG { get { return m_AttackRange; } }

    //could add crit, speed, weapon bonus, attack range

    public void AttackTarget(CharacterStats other)
    {
        m_target = other.GetComponent<MoveableCharacter>();

        if (other.HP > 0 && TargetInRange())
        {
            other.HP -= CalculateDamage(AP, other.DEF);
            logOfEvents.AddEvent(this.gameObject.name + " hit " + other.gameObject.name + " for " + CalculateDamage(AP, other.DEF) + " damage.");
            //animate taking damage && attacking;
            if (other.HP <= 0)
            {
<<<<<<< HEAD
                //EventLogger.AddEvent(this.gameObject.name + " killed: " + other.gameObject.name);
=======
                logOfEvents.AddEvent(this.gameObject.name + " killed: " + other.gameObject.name);
>>>>>>> 37061aa92e8020365b0d2f4f51df1ef976e1efce
                other.Kill();
            }
            else
            {
                logOfEvents.AddEvent(other.gameObject.name + " has " + other.HP + " HP remaining!");
            }
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

        if(TargetLocation.x == AttackerLocation.x)
        {
            if (TargetLocation.y < AttackerLocation.y + (blockDistance * this.RNG) && TargetLocation.y > AttackerLocation.y)
            {
                //TOP
                print("INRANGE TOP");
                return true;
            }

            else if (TargetLocation.y > AttackerLocation.y - (blockDistance * this.RNG) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom
                print("INRANGE BOTTOM");
                return true;
            }
        }

        else if(TargetLocation.y == AttackerLocation.y)
        {
            if(TargetLocation.x > AttackerLocation.x - (blockDistance * this.RNG) && TargetLocation.x < AttackerLocation.x)
            {
                //left
                print("INRANGE LEFT");
                return true;
            }

            else if(TargetLocation.x < AttackerLocation.x + (blockDistance * this.RNG) && TargetLocation.x > AttackerLocation.x)
            {
                //right
                print("INRANGE RIGHT");
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
            if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RNG) && TargetLocation.x < AttackerLocation.x 
                && TargetLocation.y < AttackerLocation.y + (blockDistance * this.RNG) && TargetLocation.y > AttackerLocation.y)
            {
                //If it is in range left and in range top, we know diagonally its up left
                print("INRANGE TOP LEFT");
                return true;
            }

            else if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RNG) && TargetLocation.x < AttackerLocation.x 
                && TargetLocation.y > AttackerLocation.y - (blockDistance * this.RNG) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom left
                print("INRANGE BOTTOM LEFT");
                return true;
            }

            else if (TargetLocation.x < AttackerLocation.x + (blockDistance * this.RNG) && TargetLocation.x > AttackerLocation.x 
                && TargetLocation.y < AttackerLocation.y + (blockDistance * this.RNG) && TargetLocation.y > AttackerLocation.y)
            {
                //Top right
                print("INRANGE TOP RIGHT");
                return true;
            }

            else if (TargetLocation.x > AttackerLocation.x - (blockDistance * this.RNG) && TargetLocation.x < AttackerLocation.x 
                && TargetLocation.y > AttackerLocation.y - (blockDistance * this.RNG) && TargetLocation.y < AttackerLocation.y)
            {
                //Bottom right
                print("INRANGE BOTTOM RIGHT");
                return true;
            }
        }

        print("NOT INRANGE");
        return false;
    }

    private void Awake()
    {
        logOfEvents = GameObject.FindGameObjectWithTag("EventLog").GetComponent<EventLog>();
    }
}
