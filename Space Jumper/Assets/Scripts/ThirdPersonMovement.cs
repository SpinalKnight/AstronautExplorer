using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public Transform Player;

    public float speed = 6f;
    public float gravity = -10f;
    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.8f;
    public LayerMask groundMask;
    bool isGrounded;

    private int jumpsLeft;
    public int maxJumps = 2;

    public float glideGravity;

    private Animator animator;

    public float jetPackPower = 0.1f;
    private bool level2;

    public LayerMask level2Mask;

    public float maxFuel = 100;
    public float fuel;

    private int animationParam;

    public float flipPush = 5f;

    public int doubleJumpInc = 1;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        maxJumps--;

        jumpsLeft = maxJumps;

        animator = GetComponentInChildren<Animator>();

        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {

        if (Player.transform.position.y <= -15)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        level2 = Physics.CheckSphere(groundCheck.position, groundDistance, level2Mask);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0 || vertical > 0 || horizontal < 0 || vertical < 0)
        {
            if (isGrounded)
            {
                animationParam = 1;
            }
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsLeft--;
        }
        if (isGrounded)
        {
            doubleJumpInc = 1;
            if(fuel < maxFuel)
            {
                fuel += 100 * Time.deltaTime;
            }
            jumpsLeft = maxJumps;

        }

        if (isGrounded || fuel <= 0) { 
              gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (!isGrounded && !Input.GetKeyDown(KeyCode.LeftShift) || horizontal == 0 && vertical == 0 && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (animationParam != 2)
            {
                animationParam = 0;
            }
        }

        if (Input.GetButton("Jump") && !isGrounded && fuel > 0)
        {
            gameObject.GetComponent<ParticleSystem>().enableEmission = true;

            if (velocity.y < 5f)
            {
                velocity.y += jetPackPower * Time.deltaTime;
            }
            fuel -= 100 * Time.deltaTime;
        }

        if (!Input.GetButton("Jump") && fuel > 0)
        {
            gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        if (level2)
        {
            SceneManager.LoadScene("Level2");

        }

        
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded && doubleJumpInc > 0)
        {
            animationParam = 2;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            doubleJumpInc--;
        }

        if(isGrounded && animationParam == 2){
            animationParam = 0;
        }

        animator.SetInteger("AnimationPar", animationParam);

        
    }
}
