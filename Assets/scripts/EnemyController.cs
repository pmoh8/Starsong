using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Vector3 startPt;
    public Vector3 turnPt;
    public Vector3 speed = new Vector3(3, 0, 0);

    private Rigidbody rb;
    
    public Vector3 moveDist= new Vector3(3, 0, 0);

    // Enemy pace - default : always on
    public int direction = 1; // 1 = move right, -1 = move left
    public Quaternion lookLeft = Quaternion.Euler(0, 0, 0);
    public Quaternion lookRight = Quaternion.Euler(0, 180, 0);


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPt = rb.position;
        turnPt = startPt + moveDist;
    }

    void FixedUpdate()
    {
        // Decides pace direction, 1 = Right, -1 = Left
        if (rb.position.x >= turnPt.x && direction == 1)
        {
            direction = -1;
            transform.rotation = lookRight;
        }
        else if (rb.position.x < startPt.x && direction == -1)
        {
            direction = 1;
            transform.rotation = lookLeft;
        }

        // Moves Object with Ridgebody left and right
        rb.MovePosition(rb.position + (direction * speed) * Time.deltaTime);

    }
}
