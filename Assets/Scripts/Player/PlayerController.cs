using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // components
    CharacterController cc;

    // player movement
    [SerializeField] float speed;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpSpeed = 10.0f;

    // misc
    public float speedY;

    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();
            
            if (speed < 0)
            {
                speed = 10.0f;
                throw new ArgumentException("Move speed was set to default value");
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
        }

        
        //GameManager.Instance.TestGameManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaused) return;

        float hInput = Input.GetAxisRaw("Horizontal");
        float fInput = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(hInput, 0, fInput).normalized;
        moveInput *= speed;

        speedY = (!cc.isGrounded) ? speedY -= gravity * Time.deltaTime : 0;

        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveInput.y = jumpSpeed;
            speedY = jumpSpeed;
        }

        moveInput.y = speedY;
        cc.Move(moveInput * Time.deltaTime);

    }

    
}
