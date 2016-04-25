using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    public static float distanceTraveled;

    private bool touchingPlatform;


    public Vector3[] jumpVelocity;
    public Vector3 boostVelocity;

    public float[] acceleration;
    public Material[] materials;
    public static int curCalo = 100;

    //
    //public int caloPerSecond = 1;
    //float timeLeft = 1.0f;
    //
    //-> time to lose 1 calo
    float timeLeft;
    public float timeLose1Calo = 0.5f;
    private int index = 0;
    public float gameOverY;

    private Vector3 startPosition;
    private Vector3 platformVelocity;
    private static int boosts;
	// Use this for initialization
    Animator anim;


    private string currentPlatform = "-1";
	void Start () {
        timeLeft = timeLose1Calo;
        boosts = 0;
        anim = GetComponent<Animator>();
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        GameEventManager.GamePause += GamePause;

        startPosition = transform.localPosition;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        //index = Random.Range(0, acceleration.Length);    
        GetComponent<Renderer>().material = materials[0];

        
        enabled = false;
	}

    private void GamePause()
    {
        Time.timeScale = 0;
    }
    private void InvokeRepeating(string p1, double p2, double p3)
    {
        throw new System.NotImplementedException();
    }

	// Update is called once per frame
	void Update () {

        timeLeft -= Time.deltaTime;
       
        if (timeLeft <= 0)
        {
             curCalo -= 1;
             GUIManager.SetCalo(curCalo);
             timeLeft = timeLose1Calo;
        }

        if (currentPlatform == "UpSideDown" && touchingPlatform && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))) 
        {
            upsidedown();
        }
        else if (touchingPlatform && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)))
        {
            //anim.SetTrigger("Jump");
            // anim.Play("Runner_Fly");
            GetComponent<Rigidbody>().AddForce(jumpVelocity[0], ForceMode.VelocityChange);
            touchingPlatform = false;
        }
        else if (boosts > 0 && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)))
        {
            GetComponent<Rigidbody>().AddForce(boostVelocity, ForceMode.VelocityChange);
            boosts -= 1;
            GUIManager.SetBoosts(boosts);
        }
        else if (touchingPlatform == false && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)))
        {
            brake();
        }


        transform.Translate(5f * Time.deltaTime, 0f, 0f);
        distanceTraveled = transform.localPosition.x;

       
        GUIManager.SetDistance(distanceTraveled);

        if (transform.localPosition.y < gameOverY || curCalo <= 0)
        {
            //GameEventManager.TriggerGameOver();
            GameEventManager.TriggerGamePause();
        }
	}

    void FixedUpdate()
    {
        if (touchingPlatform)
        {
            GetComponent<Rigidbody>().AddForce(acceleration[0] + platformVelocity.x, 0f, 0f, ForceMode.Acceleration);
        }
    }

    private void GameStart()
    {
        Time.timeScale = 1;
        GUIManager.SetBoosts(boosts);
        distanceTraveled = 0f;
        curCalo = 100;

        if (Physics.gravity.y > 0)
        { 
            float g = Physics.gravity.y;
            Physics.gravity = new Vector3(0, -1 * g, 0);
        }

        currentPlatform = "-1";
        GUIManager.SetDistance(distanceTraveled);
        transform.localPosition = startPosition;
        GetComponent<Renderer>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        enabled = true;
    }

    public static void AddBoost()
    {
        boosts += 1;
        GUIManager.SetBoosts(boosts);
    }

    public static void AddCalo()
    {
        curCalo += 20;
        GUIManager.SetCalo(curCalo);
    }

    private void GameOver()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        //index = Random.Range(0, acceleration.Length);
        GetComponent<Renderer>().material = materials[0];
        enabled = false;
    }

    void OnCollisionEnter( Collision c)
    {
        string lastPlatform = currentPlatform;
        platformVelocity.x = getSpeedBonusByPlatformName( c.collider.name);
        currentPlatform = c.collider.name;
        touchingPlatform = true;

        if ( Physics.gravity.y > 0 && lastPlatform == "UpSideDown")
            upsidedown();
    }

    void OnCollisionExit( Collision c)
    {
        touchingPlatform = false;
    }

    void setPlatformBonusSpeed( Vector3 add)
    {
        platformVelocity = add;
    }

    void brake()
    {
        if (touchingPlatform == false)
        {
            Debug.Log("brake = " );
            GetComponent<Rigidbody>().AddForce(0f, -100f, 0f, ForceMode.Acceleration);
        }
    }

    void upsidedown()
    {
        float g = Physics.gravity.y;
        Physics.gravity = new Vector3(0, -1 * g, 0);
    
        if( g < 0)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1.2f, transform.localPosition.z);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1.2f, transform.localPosition.z);
    }

    float getSpeedBonusByPlatformName( string name)
    {
        switch( name)
        {
            case "Regular":
                return 2f;
            case "Slow":
                return 0f;
            case "Fast":
                return 5f;
            case "UpSideDown":
                return 3f;
            case "AutoJump":
                return 3f;
            case "Block":
                return 3f;
            default:
                return 0f;
        }
    }
}
