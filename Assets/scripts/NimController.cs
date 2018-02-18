using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimController : MonoBehaviour {
    public float speed = 10;
    public float groundSpeed = 80;
    public float airSpeed = 5;
    public float jump = 1000;
    public float maxSpeed = 7;

    private Rigidbody rb;

    public int direction = 1;
    public Quaternion lookLeft = Quaternion.Euler(0, 0, 0);
    public Quaternion lookRight = Quaternion.Euler(0, 180, 0);

    private Animator animator;

    private bool multiJumpAllowed = true;
    public int maxJumps = 3;
    public int jumpCount;
    public float nextJumpTime;
    public float jumpBuffer=.2f;
    GameObject star;
    //private Transform myTransform;
    // Use this for initialization
    void Start () {
        star = GameObject.Find("Star");

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        direction = 1;
        Physics.gravity = new Vector3(0, -40F, 0);

        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (isOnGround())
        {
            jumpCount = maxJumps;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround())
        {
            Vector3 movement = new Vector3(0, jump, 0);
            rb.AddForce(movement);          
        }
        float mvmtHoriz = Input.GetAxis("Horizontal");

        if (mvmtHoriz != 0)
        {
            animator.SetInteger("State", 1);
        }
        else
        {
            animator.SetInteger("State", 0);
        }
    }

    //for the physics
    private void FixedUpdate(){
        setSpeed();

        //determing direction to face
        float mvmtHoriz = Input.GetAxis("Horizontal");
        float mvmtVert = Input.GetAxis("Vertical");
        walk(mvmtHoriz);
        if (Input.GetKeyDown(KeyCode.Z) && !isOnGround() && jumpCount > 0 && Time.time > nextJumpTime && multiJumpAllowed)
        {
            multiJump(mvmtHoriz, mvmtVert);
        }

    }
    private void walk(float mvmtHoriz)
    {
        if (rb.velocity.magnitude > maxSpeed && isOnGround())
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if (mvmtHoriz < 0 && direction == 1)
        {
            direction = -1;
            transform.rotation = lookRight;

        }
        else if (mvmtHoriz > 0 && direction == -1)
        {
            direction = 1;
            transform.rotation = lookLeft;
        }
        Vector3 movement = new Vector3(mvmtHoriz, 0, 0);
        rb.AddForce(movement * speed);
    }
    private void multiJump(float mvmtHoriz, float mvmtVert)
    {
        cancelMomentum();
        star.GetComponent<starController>().hardResetPosition();
        Vector3 mvmt = new Vector3(mvmtHoriz*.75f, mvmtVert+40f/1000f, 0) * 800;
        rb.AddForce(mvmt);
        star.GetComponent<Rigidbody>().AddForce(-.7f * mvmt);
        jumpCount--;
        nextJumpTime = Time.time + jumpBuffer;
        Debug.Log(mvmt);

    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "DeathPlane"|| col.gameObject.name == "Enemy")
        {
            death();
        }
    }
    private void death()
    {
        cancelMomentum();
        transform.position = new Vector3(0, 0, 0);
        star.transform.position = new Vector3(1, 1, 1);
    }
    private void cancelMomentum()
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }
    bool isOnGround() {
        return Physics.Raycast(transform.position, -Vector3.up, 1);
    }
    void setSpeed()
    {
        if (isOnGround())
        {
            speed = groundSpeed;
        }
        else
        {
            speed = airSpeed;
        }
    }

}
