using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlantState
{
    Disable,
    Enable//»î×Å
}


public class Plant : MonoBehaviour
{
    PlantState plantState=PlantState.Enable;
    private int HP = 100;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        this.HP -= damage;
        if(HP<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
