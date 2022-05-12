using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Enemigo : LivingEntity
{
    private UnityEngine.AI.NavMeshAgent pathfinder;
    public Transform target;
    private float myCollisionRadius;
    private float targetCollisionRadius;
    private float attackRange = 0.5f;
    private float timeUntilNextAttack = 0;
    private float timeBetweenAttacks = 2.5f;
    private bool attacking = false;
    private FirstPersonController targetEntity;
    private float damage = 1;
    private bool bEndGame = false;
    private Animator animator;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>(); 

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        targetEntity = target.GetComponent<FirstPersonController>();
        FirstPersonController.onPlayerDeath += EndGame;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!bEndGame){
            if(!attacking){
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackRange); 
                    pathfinder.SetDestination(targetPosition);

                    if(Time.time > timeUntilNextAttack){
                        timeUntilNextAttack = Time.time + timeBetweenAttacks;
                        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                        if(sqrDstToTarget <= Mathf.Pow(myCollisionRadius + targetCollisionRadius + attackRange, 2)){
                            animator.SetTrigger("Attack");
                            StartCoroutine(Attack());
                        }
                    }
            }
            if(dead){
                GameObject.Destroy(gameObject);
            }
        }
    }
    void EndGame(){
        bEndGame = true;
    }

    IEnumerator Attack(){
        if(!bEndGame){
            pathfinder.enabled = false;
            attacking = true;
            Vector3 originalPosition = transform.position;
            Vector3 distToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = target.position - distToTarget * (myCollisionRadius + targetCollisionRadius);

            float percent = 0;
            float attackSpeed = 1;

            bool hasAppliedDamage = false;
            targetEntity.gotHurt();
            while (percent <= 1){
                if(percent >= .5f && !hasAppliedDamage){
                    targetEntity.TakeDamage(damage);
                    hasAppliedDamage = true;
                }
                percent += Time.deltaTime * attackSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
                yield return null;
            }
            pathfinder.enabled = true;
            attacking = false;
        }
    }
}
