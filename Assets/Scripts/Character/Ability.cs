using UnityEngine;
using System.Collections;

public class Ability
{
    private string m_name;
    private int m_damage;
    private int m_APCost;
    private Aspects spellAspect;
    private enum Aspects
    {
        fire = 0,
        water = 1,
        earth = 2,
        air = 3,
        light = 4,
        dark = 5,
    }

    public Ability(string name, int dmg, int cost)
    {
        m_name = name;
        m_damage = dmg;
        m_APCost = cost;
        //spellAspect = aspect;
    }

    public string Name { get { return m_name; } }
    public int Damage { get { return m_damage; } }
    public int Cost { get { return m_APCost; } }

}
