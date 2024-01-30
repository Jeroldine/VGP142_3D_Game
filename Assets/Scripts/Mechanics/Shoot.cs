using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform spawnPoint;

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
    public void Fire()
    {
        Vector3 dir = transform.forward;
        dir.y = Mathf.Tan(angle * (Mathf.PI / 180));
        Debug.Log("Throw Direction: " + dir);
        dir.Normalize();
        Projectile projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Rock velocity: " + speed * dir);
        projectile.setVelocity(speed * dir);
    }
}
