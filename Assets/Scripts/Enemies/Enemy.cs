using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float gravity = 9.81f;

    CharacterController cc;
    Animator anim;
    string targetTag;
    float speedY = 0;

    bool spottedByPlayer;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (!target)
            Debug.Log("No target given");
        else
        {
            targetTag = target.tag;
            Debug.Log("Target Tag: " + targetTag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spottedByPlayer)
            return;

        Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
        Vector3 targetDir = (targetPos - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        anim.SetFloat("Speed", targetDir.magnitude);
        targetDir *= speed;

        float singleStep = turnSpeed * Time.deltaTime;
        Vector3 turnDir = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);

        if (!cc.isGrounded)
        {
            speedY -= gravity * Time.deltaTime;
            if (speedY <= -100.0f)
                speedY = -100.0f;
            targetDir.y = speedY;
        }

        Debug.DrawRay(transform.position, 10 * turnDir, Color.red);

        transform.rotation = Quaternion.LookRotation(turnDir);
        
        cc.Move(Time.deltaTime * targetDir);
    }

    public void StopMovement(bool stop = true)
    {
        if (!spottedByPlayer)
            anim.SetFloat("Speed", 0);

        spottedByPlayer = stop;
    }
}
