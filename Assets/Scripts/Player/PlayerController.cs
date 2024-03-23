using System;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // components
    CharacterController cc;
    Animator anim;
    //PlayerControls playerControls;

    // aux references
    Enemy enemy;

    // player movement
    [SerializeField] float speed;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] float turnRatio = 0.1f;
    [SerializeField] LayerMask enemyCheck;

    // controls and movement
    public float speedY; // public because I want to see it in Inspector
    private float moveInputModifier = 1.0f;

    //private void Awake()
    //{
    //    playerControls = new PlayerControls();
    //    playerControls.Console.Move.performed += MovePerformed;
    //    playerControls.Console.Jump.performed += ctx => JumpPressed();
    //    playerControls.Console.Attack.performed += ctx => AttackPressed();
    //    playerControls.Console.Kick.performed += ctx => KickPressed();
    //    playerControls.Console.Throw.performed += ctx => ThrowPressed();
    //    playerControls.Console.Look.performed += ctx => LookPerformed();
    //}

    //private void OnEnable()
    //{
    //    playerControls.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerControls.Disable();
    //}

    //private void MovePerformed(InputAction.CallbackContext ctx)
    //{
    //    //Vector2 moveInput = ctx.ReadValue<Vector2>();
    //    moveInput = ctx.ReadValue<Vector2>();

    //    ////get facing direction of freeCamera
    //    //Vector3 cameraForward = Camera.main.transform.forward;
    //    //Vector3 cameraRight = Camera.main.transform.right;

    //    //cameraForward.y = 0;
    //    //cameraRight.y = 0;

    //    //cameraForward.Normalize();
    //    //cameraRight.Normalize();

    //    //camRelMoveInput = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

    //    //camRelMoveInput.x *= speed;
    //    //camRelMoveInput.z *= speed;

    //    anim.SetFloat("Speed", moveInput.magnitude);

    //}

    //private void LookPerformed()
    //{
    //    //Vector3 cameraForward = Camera.main.transform.forward;
    //    //Vector3 cameraRight = Camera.main.transform.right;

    //    cameraForward = Camera.main.transform.forward;
    //    cameraRight = Camera.main.transform.right;

    //    cameraForward.y = 0;
    //    cameraRight.y = 0;

    //    cameraForward.Normalize();
    //    cameraRight.Normalize();

    //    //camRelMoveInput = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

    //    //camRelMoveInput.x *= speed;
    //    //camRelMoveInput.z *= speed;
    //}

    //private void JumpPressed()
    //{
    //    camRelMoveInput.y = jumpSpeed;
    //    speedY = jumpSpeed;
    //}

    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            
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

        GameManager.Instance.OnPlayerDeath.AddListener((value) => ActivatePlayerDeath(value));

        if (GameManager.Instance.isDead)
            ActivatePlayerDeath(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaused || GameManager.Instance.isDead) return;

        float hInput = Input.GetAxis("Horizontal");
        float fInput = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(hInput, fInput);


        Vector3 camRelMoveInput = GetCamRelativeMoveInput(hInput, fInput);

        FaceTravelDirection(camRelMoveInput);

        camRelMoveInput *= speed;

        // Jump
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            camRelMoveInput.y = jumpSpeed;
            speedY = jumpSpeed;
        }

        if (!cc.isGrounded) // update vertical speed if in the air
        {
            speedY -= gravity * Time.deltaTime;
            if (speedY <= -100.0f)
                speedY = -100.0f;
            camRelMoveInput.y = speedY;
        }

        if (Input.GetButtonDown("Fire1"))
            AttackPressed();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            KickPressed();

        if (Input.GetButtonDown("Fire2"))
            ThrowPressed();
            

        //camRelMoveInput.y = speedY;
        anim.SetFloat("Speed", moveInputModifier* dir.magnitude);
        cc.Move(moveInputModifier * camRelMoveInput * Time.deltaTime);

        //Glare();
    }

    private Vector3 GetCamRelativeMoveInput(float hInput, float fInput)
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        string clipName = clipinfo[0].clip.name;
        if (clipName == "Attack" || clipName == "Throw" || clipName == "Kick")
            return Vector3.zero;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return (cameraForward * fInput + cameraRight * hInput).normalized;
    }

    private void FaceTravelDirection(Vector3 CRMI)
    {
        float xCRMI = CRMI.x;
        float zCRMI = CRMI.z;
        if (( xCRMI*xCRMI + zCRMI*zCRMI ) > 0)
        {
            Vector3 lookDir = new Vector3(xCRMI, 0, zCRMI);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), turnRatio);
        }
    }

    private void AttackPressed()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipinfo[0].clip.name != "Attack")
            anim.SetTrigger("Attack");
    }

    private void KickPressed()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipinfo[0].clip.name != "Kick")
            anim.SetTrigger("Kick");
    }

    private void ThrowPressed()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipinfo[0].clip.name != "Throw")
            anim.SetTrigger("Throw");
    }

    private void ActivatePlayerDeath(bool val)
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipinfo[0].clip.name != "Zombie Death")
            anim.SetBool("Death", true);
    }

    public void ChangeMoveInputModifier(float mod)
    {
        moveInputModifier = mod;
    }

    //private void Glare()
    //{
    //    RaycastHit hitInfo;

    //    Debug.DrawLine(transform.position, transform.position + transform.forward * 10.0f, Color.red);
    //    float castDistance = 20.0f;
    //    if (Physics.BoxCast(transform.position, transform.localScale * 0.5f, transform.forward, out hitInfo, transform.rotation, castDistance, enemyCheck))
    //    {
    //        if (!enemy)
    //        {
    //            enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();
    //            enemy.StopMovement();
    //        }
    //    }
    //    else
    //    {
    //        if (enemy)
    //        {
    //            enemy.StopMovement(false);
    //            enemy = null;
    //        }
    //    }
    //}
}
