using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
public LayerMask whatIsProp;

 public ParticleSystem explosionParticle;
 public AudioSource explosionAudio;
public float MaxDamage = 100f;
public float explosionForce = 1000f;
public float lifeTime = 2f;

public float explosionRadius = 20f;

void Start() {
    Destroy(gameObject,lifeTime);

}

private void OnDestroy() {
  GameManager.instance.OnBallDestroy();    
}

private void OnTriggerEnter(Collider other) {

    Collider [] colliders = Physics.OverlapSphere(transform.position,explosionRadius,whatIsProp);

    for (int i =0;i<colliders.Length;i++){
        Rigidbody targetRigidBody = colliders[i].GetComponent<Rigidbody>();
        targetRigidBody.AddExplosionForce(explosionForce,transform.position,explosionRadius);
        prop targetProp = colliders[i].GetComponent<prop>();
        float damage = CalculateDamage(colliders[i].transform.position);
        targetProp.TakeDamage(damage);
    }




    explosionParticle.transform.parent = null;
    explosionParticle.Play();
    explosionAudio.Play();

    
    
    Destroy(explosionParticle.gameObject,explosionParticle.duration);
    Destroy(gameObject);
    
}

private float CalculateDamage(Vector3 targetPosition){

    Vector3 explosionToTarget = targetPosition - transform.position;
    float distance = explosionToTarget.magnitude;
    float edgeToCenterDistance = explosionRadius - distance;
    float percentage = edgeToCenterDistance/explosionRadius;

    float damage = Mathf.Max(0,MaxDamage*percentage);

    return damage;

}

}
