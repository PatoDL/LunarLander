using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    public float altitude;
    public float fuel = 3000f;
    public delegate void OnRocketDeath();
    public static OnRocketDeath RocketDeath;

    public delegate void OnRocketSuccessfulLanding();
    public static OnRocketSuccessfulLanding RocketWin;

    public Rigidbody2D rig;
    SpriteRenderer spriteR;
    public float impulseSpeed;
    public float torqueSpeed;
    new ParticleSystem particleSystem;

    Vector3 cameraStartPos;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        RocketDeath += KillRocket;
        cameraStartPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovementSpaceLimiter();
        CheckIfNearTerrain();
        altitude = -(TerrainGenerator.minY - transform.position.y);
        Debug.Log(TerrainGenerator.minY + "," + altitude);
    }

    const int raycastAmount = 3;
    float[] wingDistance = new float[raycastAmount];

    void CheckIfNearTerrain()
    {
        int layerMask = 1 << 8;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector3.down, 1.5f, layerMask);

        if (hitInfo)
        {
            Camera.main.orthographicSize = 1.37f;
            Vector3 cameraZoomPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = cameraZoomPosition;
            wingDistance[2] = hitInfo.distance;
            CheckIfCorrectLanding();
        }
        else
        {
            Camera.main.orthographicSize = 5f;
            Camera.main.transform.position = cameraStartPos;
        }
    }

    void CheckIfCorrectLanding()
    {
        int layerMask = 1 << 8;
        RaycastHit2D[] hitInfo = new RaycastHit2D[raycastAmount];

        float scaleReductor = 10;
        hitInfo[0] = Physics2D.Raycast(transform.position + transform.right/scaleReductor, -transform.up, 1, layerMask);
        hitInfo[1] = Physics2D.Raycast(transform.position - transform.right/scaleReductor, -transform.up, 1, layerMask);

        for (int i=0;i<raycastAmount-1;i++)
        {
            if (hitInfo[i])
                wingDistance[i] = hitInfo[i].distance;
            

        }
    }

    void MovementSpaceLimiter()
    {
        Bounds bounds = CameraUtils.OrthographicBounds();

        bool outleft = transform.position.x < bounds.min.x + transform.localScale.y;
        if (outleft)
        {
            transform.position = new Vector3(bounds.min.x + transform.localScale.y, transform.position.y);

        }

        bool outright = transform.position.x > bounds.max.x - transform.localScale.y;
        if (outright)
        {
            transform.position = new Vector3(bounds.max.x - transform.localScale.y, transform.position.y);

        }

        bool outup = transform.position.y > bounds.max.y - transform.localScale.y;
        if (outup)
        {
            transform.position = new Vector3(transform.position.x, bounds.max.y - transform.localScale.y);

        }
    }

    bool validateMovement = true;
    void FixedUpdate()
    {
        if (validateMovement)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
            {
                rig.AddForce(transform.up * impulseSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
                fuel -= Time.fixedDeltaTime;
                if (fuel <= 0f)
                    validateMovement = false;
                if (!particleSystem.isPlaying)
                    particleSystem.Play();
                else
                    particleSystem.Emit(1);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rig.AddTorque(-torqueSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rig.AddTorque(torqueSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            validateMovement = false;
            float velValue = 0.05f;
            bool lose = false;
            if (rig.velocity.x < velValue && rig.velocity.x > -velValue && rig.velocity.y < velValue && rig.velocity.y > -velValue)
            {
                if (transform.rotation.eulerAngles.z < 3f || transform.rotation.eulerAngles.z > 360f-3f)
                {
                    bool validDistance = true;
                    for (int i = 0; i < raycastAmount; i++)
                    {
                        for (int j = 0; j < raycastAmount; j++)
                        {
                            if (i != j)
                            {
                                if (!(wingDistance[i] < wingDistance[j] + 0.1))
                                {
                                    validDistance = false;
                                }
                            }
                        }
                    }
                    if (validDistance)
                    {
                        RocketWin();
                    }
                    else
                    {
                        lose = true;
                    }
                }
                else
                {
                    lose = true;
                }
            }
            else
            {
                lose = true;
            }

            if(lose)
            {
                RocketDeath();
            }
        }
    }

    void KillRocket()
    {
        spriteR.color = Color.red;
        Debug.Log("died");
    }
}
