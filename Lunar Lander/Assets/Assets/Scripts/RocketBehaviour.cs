using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    public delegate void OnRocketDeath();
    public static OnRocketDeath rocketDeath;
    Rigidbody2D rig;
    SpriteRenderer spriteR;
    public float impulseSpeed;
    public float torqueSpeed;
    ParticleSystem particleSystem;
    BoxCollider2D dMeasurer;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        dMeasurer = GetComponentInChildren<BoxCollider2D>();
        rocketDeath += KillRocket;
    }

    // Update is called once per frame
    void Update()
    {
        MovementSpaceLimiter();
    }

    void MovementSpaceLimiter()
    {
        Bounds bounds = CameraUtils.OrthographicBounds();
        
        bool outleft = transform.position.x < bounds.min.x + transform.localScale.y;
        if(outleft)
        {
            transform.position = new Vector3(bounds.min.x + transform.localScale.y,transform.position.y);
            
        }

        bool outright = transform.position.x > bounds.max.x - transform.localScale.y;
        if(outright)
        {
            transform.position = new Vector3(bounds.max.x - transform.localScale.y, transform.position.y);
           
        }

        bool outup = transform.position.y > bounds.max.y - transform.localScale.y;
        if (outup)
        {
            transform.position = new Vector3(transform.position.x, bounds.max.y - transform.localScale.y);
            
        }
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            rig.AddForce(transform.up * impulseSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            if (!particleSystem.isPlaying)
                particleSystem.Play();
            else
                particleSystem.Emit(1);
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

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Terrain")
        {
            Debug.Log(rig.velocity);
            rig.gravityScale *= 5f;
            if (rig.velocity.y > 0.5f || rig.velocity.y < -0.5f || rig.velocity.x > 0.5f || rig.velocity.x < -0.5f)
            {
                rocketDeath();
            }
        }
    }
    void KillRocket()
    {
        Debug.Log("died");
    }
}
