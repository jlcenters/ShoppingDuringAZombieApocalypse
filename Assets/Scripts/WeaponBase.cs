using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 
    WEAPON FUNCTIONS:
        core weapon class for instantiation
 *
 *
 */
public class WeaponBase
{
    public readonly int range;
    public readonly int damage;
    public readonly int altDamage; //for ranged, weapon strike dmg; for melee, charged dmg
    public readonly float attackCooldown; //seconds of cooldown

    //constructor
    public WeaponBase(int range, int damage, int altDamage, float attackCooldown)
    {
        this.range = range;
        this.damage = damage;
        this.altDamage = altDamage;
        this.attackCooldown = attackCooldown;
    }

    override public string ToString()
    {
        return "WEAPON STATS=>  [range:" + range + ", damage:" + damage + ", alt damage:" + altDamage + ", cooldown timer:" + attackCooldown;
    }
}

