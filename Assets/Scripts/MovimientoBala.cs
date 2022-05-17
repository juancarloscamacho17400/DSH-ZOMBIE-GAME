using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoBala : MonoBehaviour
{

    public float velocidad = 10.0f;
    public LayerMask collisionMask;
    public delegate void OnDeath();
    public static event OnDeath OnDeathAnother;
    private float damage = 1;
    private LayerMask LayerEnemy;
    // Start is called before the first frame update
    void Start()
    {
        LayerEnemy = LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = Time.deltaTime * velocidad;
        transform.Translate(Vector3.forward * moveDistance);
        CheckCollision(moveDistance);
    }

    void CheckCollision(float mDistance){
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, mDistance, collisionMask, QueryTriggerInteraction.Collide)){
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit){
        GameObject.Destroy(gameObject); //Las balas desaparecen al chocar contra cualquier objeto perteneciente a uno de los layers en collisionMask
        if(hit.transform.gameObject.layer == LayerEnemy){ //Si es un enemigo, recibe daño
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            if(damageableObject != null){
                damageableObject.TakeHit(damage, hit);
                if (OnDeathAnother != null && damageableObject.Dead()){
                    OnDeathAnother();
                }
            }
        }
    }
}
