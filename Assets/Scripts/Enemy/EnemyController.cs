using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 
    ENEMY CONTROLLER FUNCTIONS:
        stores enemy data
        methods for attacking
        methods for receiving damage
        methods for death sequence
 *
 *
 */
public class EnemyController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Color dmgColor;
    [SerializeField] private Color defaultColor;
    public int Health { get; private set; }



    private void Awake()
    {
        ResetData();
        defaultColor = GetComponentInChildren<Renderer>().material.color;
    }



    private void ResetData()
    {
        Health = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log(Health);

        if(Health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            //play animation to hurt object
            StartCoroutine(TakeDamageAnimation());
        }
    }
    private IEnumerator TakeDamageAnimation()
    {
        GetComponentInChildren<Renderer>().material.SetColor("_Color", dmgColor);
        yield return new WaitForSeconds(0.5f);
        GetComponentInChildren<Renderer>().material.SetColor("_Color", defaultColor);
    }
}
