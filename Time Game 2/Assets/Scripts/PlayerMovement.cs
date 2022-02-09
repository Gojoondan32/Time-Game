using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 10f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float jumpHeight = 20f;
    [SerializeField] private float dashSpeed = 20f;

    public bool canDash = true;

    private Vector3 velocity;

    private float inputX;
    private float inputZ;

    [Header("Ground Variables")]
    [SerializeField]private bool isGrounded;
    public Transform groundCheck;
    private float groundDistance = 0.4f;

    public static Vector3 move;
    private bool isAttacking = false;

    private float cooldown = 0.5f;
    private float currentTime = 0f;

    [SerializeField] private Transform cam;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Vector3 dashVelocity;


    public float dashTime;
    public static Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        //Get the character controller component
        controller = GetComponent<CharacterController>();
        canDash = true;
        isAttacking = false;
        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        
        if (Input.GetButtonDown("Jump"))
        {
            GroundCheck();
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * gravity);
            }
            
        }

        if(currentTime >= cooldown)
        {
            currentTime = cooldown;
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                //Dash(inputX, inputZ);
                //StartCoroutine(DashTime());
                StartCoroutine(NewDash());
                isAttacking = false;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }


        if (!isAttacking)
        {
            
        }
        MovePlayer(inputX, inputZ);
        AddGravity();

    }

    private void MovePlayer(float inputX, float inputZ)
    {
        //Change the player input into a direction
        Vector3 direction = new Vector3(inputX, 0, inputZ);
        move = direction * speed;

        //move = transform.TransformDirection(move);
        move = cam.forward * move.z + cam.right * move.x;
        move.y = 0;


        //Move the player acording to the player input
        //Uses unscaledDeltaTime to allow the player to move when time is stopped
        controller.Move(move * Time.unscaledDeltaTime);

        if (!isAttacking)
        {
            if (direction != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }
        }
        
    }

    private void AddGravity()
    {
        velocity.y -= gravity * Time.unscaledDeltaTime;

        //Apply the gravity force on the velocity
        controller.Move(velocity * Time.unscaledDeltaTime);
    }

    private void GroundCheck()
    {
        //Check if the player is on the ground
        LayerMask groundMask = LayerMask.GetMask("Ground");
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
    }


    public void ChargeEnemy(Transform enemy)
    {
        //Snap to look at the enemy
        Vector3 targetDirection = new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
        transform.LookAt(targetDirection);
        isAttacking = true;
        StartCoroutine(AttackingTime());
    }
    private IEnumerator AttackingTime()
    {
        //Used to wait for attack animations to finish
        yield return new WaitForSeconds(3f);
        isAttacking = false;
    }



    private IEnumerator NewDash()
    {
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            controller.Move(moveDir * dashSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
    }

}
