using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCharacter : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;
    public float sideSpeed = 8.0f;

    Rigidbody r;
    bool grounded = false;
    Vector3 defaultScale;
    bool crouch = false;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        r.freezeRotation = true;
        r.useGravity = false;
        defaultScale = transform.localScale;
    }

    void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            r.velocity = new Vector3(r.velocity.x, CalculateJumpVerticalSpeed(), r.velocity.z);
        }

        //Crouch
        crouch = Input.GetKey(KeyCode.S);
        if (crouch)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime * 7);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * 7);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // apply gravity manually 
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));

        grounded = false;

        float horiz = Input.GetAxis ("Horizontal");
        Vector3 moveSide = transform.right * horiz;
        moveSide *= sideSpeed * Time.deltaTime;

        transform.localPosition += moveSide;
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            //print("GameOver!");
            Destroy(gameObject);
            RegenerateGround.instance.gameOver = true;
        }

        if(collision.gameObject.tag == "wall")
        {
            //print("GameOver!");
            Destroy(gameObject);
            RegenerateGround.instance.gameOver = true;
        }
    }
}
