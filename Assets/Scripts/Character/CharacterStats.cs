using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public int m_AttackPower;
    public int m_HealthPoints;
    public int m_DefensePower;

    public int AP { get { return m_AttackPower; } }
    public int HP { get { return m_HealthPoints; } set { m_HealthPoints = value; } }
    public int DEF { get { return m_DefensePower; } }

    //could add crit, speed, weapon bonus, attack range

    public void AttackTarget(CharacterStats other)
    {
        if(other.HP > 0)
        {
            other.HP -= CalculateDamage(AP, other.DEF);


            print(this.gameObject.name + " hit " + other.gameObject.name + " for " + CalculateDamage(AP, other.DEF) + " damage.");

            //animate taking damage && attacking;
            if (other.HP <= 0)
            {
                print(this.gameObject.name + " killed: " + other.gameObject.name);
                other.Kill();
            }

            else
            {
                print(other.gameObject.name + " has " + other.HP + " HP remaining!");
            }
        }
    }

    public void Kill()
    {
        //Play death animation if necessary
        //remove character from field;
        Destroy(this.gameObject);
    }

    private int CalculateDamage(int Attack, int EnemyDefense)
    {
        return Attack - EnemyDefense;
    }


}
