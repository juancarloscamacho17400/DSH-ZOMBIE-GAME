using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Enemigo : LivingEntity
{
    private UnityEngine.AI.NavMeshAgent pathfinder;
    private Transform target;
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
    private AudioSource m_AudioSource;
    public AudioClip[] m_TakeDamage;
    public AudioClip[] m_DeathSound;
    public AudioClip[] m_AttackSound;
    public AudioClip[] m_IdleSound;
    public AudioClip[] m_FootstepSounds;
    public ParticleSystem impactBlood;
    private bool dying;
    private float nextStep = 0f;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        m_AudioSource = GetComponent<AudioSource>();
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        targetEntity = target.GetComponent<FirstPersonController>();
        FirstPersonController.onPlayerDeath += EndGame;
        animator = GetComponent<Animator>();
        dying = false;
        int n = Random.Range(0, m_IdleSound.Length);
        m_AudioSource.clip = m_IdleSound[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
    }

    // Update is called once per frame
    void Update()
    {   
        if(!bEndGame){
            if(!attacking && !dying){
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackRange); 
                    ProgressStepCycle(1f);
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
            if(dying){
                pathfinder.enabled = false;
            }
            if(dead){
                GameObject.Destroy(gameObject);
            }
        }
    }
    void EndGame(){
        bEndGame = true;
    }

    public bool Dying(){
        return dying;
    }
    public override void TakeDamage(float damage){
        impactBlood.Play();
        health -= damage;
        if(health <= 0 && !dead){
            StartCoroutine(Die());
        }
        else {
            int n = Random.Range(0, m_TakeDamage.Length);
            m_AudioSource.clip = m_TakeDamage[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
        }
    }

    public override IEnumerator Die()
    {
        if(!dying){
            FirstPersonController.changeScore(2);
            int n = Random.Range(0, m_DeathSound.Length);
            m_AudioSource.clip = m_DeathSound[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            dying = true;
            yield return new WaitForSeconds(1);
            dead = true;
        }
    }

    private void ProgressStepCycle(float speed)
        {
            if (Time.time < nextStep)
            {
                return;
            }

            nextStep = Time.time + speed;
            PlayFootStepAudio();
        }
    
    private void PlayFootStepAudio()
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            //m_AudioSource.volume = 1;
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }

    IEnumerator Attack(){
        if(!bEndGame && !dying){
            pathfinder.enabled = false;
            attacking = true;
            Vector3 originalPosition = transform.position;
            Vector3 distToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = target.position - distToTarget * (myCollisionRadius + targetCollisionRadius);

            float percent = 0;
            float attackSpeed = 1;

            bool hasAppliedDamage = false;
            int n = Random.Range(0, m_AttackSound.Length);
            m_AudioSource.clip = m_AttackSound[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
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
