using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable{
    protected bool dead;
    protected float health;
    public float initialHealth;
    public ParticleSystem impactBlood;
    public void TakeHit(float damage, RaycastHit hit){
        TakeDamage(damage); 
    }

    public void TakeDamage(float damage){
        impactBlood.Play();
        health -= damage;
        if(health <= 0 && !dead){
            Die();
        }
    }

    public bool Dead(){
        return dead;
    }

    public void Die(){
        dead = true;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = initialHealth;
        dead = false;
    }
}

