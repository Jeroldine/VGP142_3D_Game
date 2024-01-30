using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float lifeTime;

    float xSpeed;
    float ySpeed;
    float zSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (lifeTime <= 0)
            lifeTime = 2.0f;

        Destroy(gameObject, lifeTime);
    }

    public void setVelocity(Vector3 v)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(v.x, v.y, v.z);
    }

    // collision handling below

}
