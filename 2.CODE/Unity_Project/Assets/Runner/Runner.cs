using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

    public static float distanceTraveled;

    private bool touchingPlatform;

    public Vector3[] jumpVelocity;

    public Vector3 boostVelocity;
    public float[] acceleration;
    public Material[] materials;
    private int index = 0;

    public float gameOverY;

    private Vector3 startPosition;

    private static int boosts;
	// Use this for initialization

    Animator anim;
	void Start () {

        boosts = 0;
        anim = GetComponent<Animator>();
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        startPosition = transform.localPosition;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        index = Random.Range(0, acceleration.Length);
        Debug.Log("Index = " + index);
        GetComponent<Renderer>().material = materials[index];
        enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (touchingPlatform && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)))
        {
            anim.SetTrigger("Jump");
            anim.Play("Runner_Fly");
            GetComponent<Rigidbody>().AddForce(jumpVelocity[index], ForceMode.VelocityChange);
            touchingPlatform = false;
        }
        else if (boosts > 0 && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)))
        {
            GetComponent<Rigidbody>().AddForce(boostVelocity, ForceMode.VelocityChange);
            boosts -= 1;
            GUIManager.SetBoosts(boosts);
        }

        transform.Translate(5f * Time.deltaTime, 0f, 0f);
        distanceTraveled = transform.localPosition.x;
        GUIManager.SetDistance(distanceTraveled);

        if (transform.localPosition.y < gameOverY)
        {
            GameEventManager.TriggerGameOver();
        }
	}

    void FixedUpdate()
    {
        if (touchingPlatform)
        {
            GetComponent<Rigidbody>().AddForce(acceleration[index], 0f, 0f, ForceMode.Acceleration);
        }
    }

    private void GameStart()
    {
        GUIManager.SetBoosts(boosts);
        distanceTraveled = 0f;
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

    private void GameOver()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        index = Random.Range(0, acceleration.Length);
        GetComponent<Renderer>().material = materials[index];
        Debug.Log("Index Over= " + index);
        enabled = false;
    }

    void OnCollisionEnter()
    {
        touchingPlatform = true;
    }

    void OnCollisionExit()
    {
        touchingPlatform = false;
    }
}
