using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public int m_AttackPower;
    public int m_HealthPoints;
    public int m_DefensePower;

    [Header("Target Info")]
    public MoveableCharacter m_target;

    public int AP { get { return m_AttackPower; } }
    public int HP { get { return m_HealthPoints; } set { m_HealthPoints = value; } }
    public int DEF { get { return m_DefensePower; } }

    //could add crit, speed, weapon bonus, attack range

    public void AttackTarget(CharacterStats other)
    {
        m_target = other.GetComponent<MoveableCharacter>();

        if (other.HP > 0 && TargetInRange() )
        {
        
            if(this.name.Contains("Player"))
            {
                /*GameObject[] tempArray = new GameObject[EnemyCharacters.Instance.TeamSize - 1];
                //GameObject tempGO;

                for(int i = 0; i < tempArray.Length + 1; i++ )
                {
                    if(EnemyCharacters.Instance.Team[i])
                    {
                        
                    }

                    else
                    {
                        //tempArray = EnemyCharacters.Instance.Team[i].gameObject;
                    }
                }*/
            }

            other.HP -= CalculateDamage(AP, other.DEF);


            print(this.gameObject.name + " hit " + other.gameObject.name + " for " + CalculateDamage(AP, other.DEF) + " damage.");
            StatusManager.Instance.text.text = this.gameObject.name + " hit " + other.gameObject.name + " for " + CalculateDamage(AP, other.DEF) + " damage.";

            //animate taking damage && attacking;
            if (other.HP <= 0)
            {
                print(this.gameObject.name + " killed: " + other.gameObject.name);
                StatusManager.Instance.text.text = this.gameObject.name + " killed: " + other.gameObject.name;
                //remove other.gameobject from array;
                other.Kill();
            }

            else
            {
                print(other.gameObject.name + " has " + other.HP + " HP remaining!");
                StatusManager.Instance.text.text = other.gameObject.name + " has " + other.HP + " HP remaining!";
            }
        }

        m_target = null;
    }

    public void Kill()
    {
        //Play death animation if necessary
        //remove character from field;
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

        if(TargetLocation.x >= AttackerLocation.x - 1) // - RANGE
        {
            //check to the left
            //if above is true, target is withn range
            return true;
        }

        return false;
    }
}
