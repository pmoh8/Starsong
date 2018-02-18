using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starController : MonoBehaviour {
    private Rigidbody rb;
    GameObject player;
    float offset = 1f;
    public Vector3 speed = new Vector3(3, 0, 0);
    private bool attack = false;
    private float returnTime;
    private float nextFire;
    NimController playerScript;
    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Nim");
        playerScript = player.GetComponent<NimController>();
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }
    // Update is called once per frame
    void FixedUpdate () {
        
        Vector3 destination = player.transform.position;
        if (Input.GetKeyDown(KeyCode.X) && Time.time > nextFire)
        {
            attack = true;
            returnTime = Time.time + .05f;
            nextFire = Time.time + .5f;
        }

        if (attack)
        {
            destination.x = destination.x + 20 * playerScript.direction;
            destination.y = destination.y - .5f;
            transform.position = Vector3.Lerp(rb.position, destination, 5*Time.deltaTime);
            if (Time.time>returnTime)
            {
                attack = false;
            }
        }
        else
        {
            resetPosition();
        }

    }
    void resetPosition()
    {
        Vector3 destination = player.transform.position;
        destination.x = destination.x - offset * playerScript.direction;
        destination.y = destination.y + .2f;
        destination.z = destination.z - .2f;
        transform.position = Vector3.Lerp(rb.position, destination, 3 * Time.deltaTime);
    }
    public void hardResetPosition()
    {
        Vector3 destination = player.transform.position;
        destination.x = destination.x - offset * playerScript.direction;
        destination.y = destination.y + .2f;
        destination.z = destination.z - .2f;
        transform.position = destination;
    }
}
