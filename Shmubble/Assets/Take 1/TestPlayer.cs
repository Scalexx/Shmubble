using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

    [Range (1,10)]
    public float jumpVelocity;
	public float moveSpeed = 7.0f ;
	int health = 3 ;

    Rigidbody rb;
    private bool isGrounded;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        // Moves Left and right along x Axis                             // Left/Right
        //transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* moveSpeed);  
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y, 0);
	}
    
    void Update ()
    {
        // Jump??
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector3.up * jumpVelocity;
        }

        // Lose health when hit


        // Die
        if (health <= 0)
        {

        }
    }

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            health -= 1;
        }
    }
}
