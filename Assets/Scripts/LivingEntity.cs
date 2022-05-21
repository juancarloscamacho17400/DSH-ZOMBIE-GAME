using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable{
    protected bool dead;
    protected float health;
    public float initialHealth;

    public void TakeHit(float damage, RaycastHit hit){
        TakeDamage(damage); 
    }

    virtual public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0 && !dead){
            StartCoroutine(Die());
        }
    }

    public bool Dead(){
        return dead;
    }

    virtual public IEnumerator Die(){
        yield return null; 
        dead = true;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = initialHealth;
        dead = false;
    }
}

