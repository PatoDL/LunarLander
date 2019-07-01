using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    public float altitude;
    public float fuel = 3000f;
    public Vector3 startPosition;

    public delegate void OnRocketDeath();
    public static OnRocketDeath RocketDeath;

    public delegate void OnRocketSuccessfulLanding();
    public static OnRocketSuccessfulLanding RocketWin;

    public delegate void OnRocketResultShowed(bool lose);
    public static OnRocketResultShowed ShowResult;

    public Rigidbody2D rig;
    SpriteRenderer spriteR;
    public float impulseSpeed;
    public float torqueSpeed;
    public float gravityScale = 0.05f;
    new ParticleSystem particleSystem;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //g = GameObject.Find("GameManager").GetComponent<GameManager>();
        rig = GetComponent<Rigidbody2D>();
        GameManager.ReloadReferences();
        spriteR = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        RocketDeath += KillRocket;
        fuel = 3000f;
        GameManager.PauseGame += PauseGame;
        GameHUD.ReturnToMenu += ResumeGame;
        //GameManager.PauseGame += ResumeGame;
        GameHUD.RePlay += PauseGame;
        GameManager.ResumeGame = ResumeGame;
        startPosition = transform.position;
        RocketDeath += RestartFuel;
    }

    void Update()
    {
        MovementSpaceLimiter();
        CheckIfNearTerrain();
        altitude = -(TerrainGenerator.minY - transform.position.y);
        rig.gravityScale = gravityScale;
    }

    void RestartFuel()
    {
        fuel = 3000;
    }

    const int raycastAmount = 3;
    float[] wingDistance = new float[raycastAmount];

    void CheckIfNearTerrain()
    {
        int layerMask = 1 << 8;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector3.down, 1.5f, layerMask);

        if (hitInfo)
        {
            //;
            GameManager.Get().ZoomCamera(true);
            wingDistance[2] = hitInfo.distance;
            CheckIfCorrectLanding();
        }
        else
        {
            GameManager.Get().ZoomCamera(false);
        }
    }

    private void OnDestroy()
    {
        RocketDeath -= KillRocket;
        RocketDeath -= RestartFuel;
        GameManager.PauseGame -= PauseGame;
        //GameManager.PauseGame -= ResumeGame;
        GameHUD.RePlay -= PauseGame;
        GameHUD.ReturnToMenu -= ResumeGame;
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

        bool outOfScreen = false;

        bool outleft = transform.position.x < bounds.min.x + transform.localScale.y;
        if (outleft)
        {
            transform.position = new Vector3(bounds.min.x + transform.localScale.y, transform.position.y);
            outOfScreen = true;
        }

        bool outright = transform.position.x > bounds.max.x - transform.localScale.y;
        if (outright)
        {
            transform.position = new Vector3(bounds.max.x - transform.localScale.y, transform.position.y);
            outOfScreen = true;
        }

        bool outup = transform.position.y > bounds.max.y - transform.localScale.y;
        if (outup)
        {
            transform.position = new Vector3(transform.position.x, bounds.max.y - transform.localScale.y);
            outOfScreen = true;
        }

        if(outOfScreen)
        {
            RocketDeath();
            ShowResult(outOfScreen);
            validateMovement = false;
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
                {
                    validateMovement = false;
                }
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
        if (validateMovement)
        {
            if (col.gameObject.tag == "Terrain")
            {
                validateMovement = false;
                float velValue = 0.05f;
                bool lose = false;
                if (rig.velocity.x < velValue && rig.velocity.x > -velValue && rig.velocity.y < velValue && rig.velocity.y > -velValue)
                {
                    if (transform.rotation.eulerAngles.z < 3f || transform.rotation.eulerAngles.z > 360f - 3f)
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

                if (lose)
                {
                    RocketDeath();
                }

                ShowResult(lose);
            }
        }
    }

    void KillRocket()
    {
        //;
        spriteR.color = Color.red;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void RestartRocket()
    {
        transform.position = startPosition;
        rig.velocity = Vector3.zero;
        transform.rotation = new Quaternion();
        spriteR.color = Color.white;
        validateMovement = true;
    }
}
