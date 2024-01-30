using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // components
    CharacterController cc;
    Animator anim;
    PlayerControls playerControls;
    Shoot shoot;

    // aux references
    Enemy enemy;

    // player movement
    [SerializeField] float speed;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] float turnRatio = 0.1f;
    [SerializeField] LayerMask enemyCheck;

    // misc
    public float speedY;
    //Vector3 camRelMoveInput;

    private void Awake()
    {
        playerControls = new PlayerControls();
        //playerControls.Console.Move.performed += MovePerformed;
        //playerControls.Console.Jump.performed += ctx => JumpPressed();
        //playerControls.Console.Attack.performed += ctx => AttackPressed(); ;
    }

    private void AttackPressed()
    {
        throw new NotImplementedException();
    }

    private void JumpPressed()
    {
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    //private void MovePerformed(InputAction.CallbackContext ctx)
    //{
    //    Vector2 moveInput = ctx.ReadValue<Vector2>();
    //    Debug.Log(moveInput);
    //    Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);

    //    //get facing direction of freeCamera
    //    Vector3 cameraForward = Camera.main.transform.forward;
    //    Vector3 cameraRight = Camera.main.transform.right;

    //    cameraForward.y = 0;
    //    cameraRight.y = 0;

    //    cameraForward.Normalize();
    //    cameraRight.Normalize();

    //    camRelMoveInput = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

    //    //if (camRelMoveInput.magnitude > 0)
    //    //{
    //    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(camRelMoveInput), turnRatio);
    //    //}

    //    camRelMoveInput *= speed;

    //    anim.SetFloat("Speed", dir.magnitude);


    //}

    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            shoot = GetComponent<Shoot>();
            
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

        //Debug.Log("camRelMoveInput = " + camRelMoveInput.magnitude);
        //GameManager.Instance.TestGameManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaused) return;

        float hInput = Input.GetAxis("Horizontal");
        float fInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(hInput, 0, fInput);

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 camRelMoveInput = (cameraForward * fInput + cameraRight * hInput).normalized;

        if (camRelMoveInput.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(camRelMoveInput), turnRatio);
        }

        camRelMoveInput *= speed;

        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            camRelMoveInput.y = jumpSpeed;
            speedY = jumpSpeed;
        }
        else if (!cc.isGrounded)
        {
            speedY -= gravity * Time.deltaTime;
            if (speedY <= -100.0f)
                speedY = -100.0f;
            camRelMoveInput.y = speedY;
        }

        if  (Input.GetButtonDown("Fire1"))
        {
            shoot.Fire();
        }

        camRelMoveInput.y = speedY;
        anim.SetFloat("Speed", dir.magnitude);
        cc.Move(camRelMoveInput * Time.deltaTime);

        //Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawLine(transform.position, transform.position + transform.forward * 10.0f, Color.red);
        //Physics.BoxCast(m_Collider.bounds.center, transform.localScale * 0.5f, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        //if (Physics.Raycast(ray, out hitInfo, 10.0f, enemyCheck))
        if (Physics.BoxCast(transform.position, transform.localScale * 0.5f, transform.forward, out hitInfo, transform.rotation, 20.0f, enemyCheck))
        {
            if (!enemy)
            {
                enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();
                enemy.StopMovement();
            }
        }
        else
        {
            if (enemy)
            {
                enemy.StopMovement(false);
                enemy = null;
            }
        }
    }

    
}
