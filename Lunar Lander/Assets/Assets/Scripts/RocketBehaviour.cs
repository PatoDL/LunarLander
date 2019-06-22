using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    Rigidbody2D rig;
    SpriteRenderer spriteR;
    public float impulseSpeed;
    public float torqueSpeed;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementSpaceLimiter();
    }

    void MovementSpaceLimiter()
    {
        Bounds bounds = CameraUtils.OrthographicBounds();

        bool outOfLimits = false;
        
        bool outleft = transform.position.x < bounds.min.x + transform.localScale.y;
        if(outleft)
        {
            transform.position = new Vector3(bounds.min.x + transform.localScale.y,transform.position.y);
            outOfLimits = true;
        }

        bool outright = transform.position.x > bounds.max.x - transform.localScale.y;
        if(outright)
        {
            transform.position = new Vector3(bounds.max.x - transform.localScale.y, transform.position.y);
            outOfLimits = true;
        }

        bool outup = transform.position.y > bounds.max.y - transform.localScale.y;
        if (outup)
        {
            transform.position = new Vector3(transform.position.x, bounds.max.y - transform.localScale.y);
            outOfLimits = true;
        }

        if(outOfLimits)
        {
            rig.gravityScale = 0;
            rig.velocity = new Vector2(0, 0);
        }
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            rig.AddForce(transform.up * impulseSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            rig.AddTorque(-torqueSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rig.AddTorque(torqueSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }

    
}
