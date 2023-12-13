using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 
    WEAPONS INVENTORY FUNCTIONS:
        stores all weapons in player inventory
        handles adding, removing weapons
 *
 *
 */
public enum WeaponTypes
{
    Primary, Secondary, Melee
}
public class WeaponsInventory : MonoBehaviour
{
    public Dictionary<WeaponTypes, WeaponBase> weapons = new();
    
    public void AddToInventory(WeaponTypes type, WeaponBase weapon)
    {
        //if weapon of same type already exists in inventory, replace; else, add
        if(weapons.ContainsKey(type))
        {
            //weapons[type] = weapon;
            weapons.Remove(type);
            Debug.Log("old weapon removed");
        }

        weapons.Add(type, weapon);
        Debug.Log("weapon added");
    }
    public void RemoveFromInventory(WeaponTypes weapon)
    {
        //if weapon type is being used, remove; else, send console message
        if (weapons.ContainsKey(weapon))
        {
            weapons.Remove(weapon);
        }
        else
        {
            Debug.Log("weapon not found in this slot");
        }
    }
}
