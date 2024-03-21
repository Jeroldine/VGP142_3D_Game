using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] GameObject hitBox;

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform attackHitbox;
    [SerializeField] Transform kickHitbox;
    [SerializeField] float hitboxLifetime;

    [SerializeField] float speed;
    [SerializeField] float angle; // in degrees

    // Start is called before the first frame update
    void Start()
    {
        if (speed <= 0)
            speed = 10.0f;
        if (!spawnPoint)
            Debug.Log("No Projectile Spawn Point given");
    }

    // Update is called once per frame
    public void ThrowObject()
    {
        Vector3 dir = transform.forward;
        dir.y = Mathf.Tan(angle * (Mathf.PI / 180));
        Debug.Log("Throw Direction: " + dir);
        dir.Normalize();
        Projectile projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Rock velocity: " + speed * dir);
        projectile.setVelocity(speed * dir);
    }

    public void ActivateAttackHitbox()
    {
        //GameObject attackHB = Instantiate(hitBox, attackHitbox.position, Quaternion.identity);
        //Destroy(attackHB, hitboxLifetime);
        attackHitbox.gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    public void DeactivateAttackHitbox()
    {
        attackHitbox.gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    public void ActivateKickHitBox()
    {
        //GameObject kickHB = Instantiate(hitBox, kickHitbox.position, Quaternion.identity);
        //Destroy(kickHB, hitboxLifetime);
        kickHitbox.gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    public void DeactivateKickHitBox()
    {
        kickHitbox.gameObject.GetComponent<SphereCollider>().enabled = false;
    }
}
