using UnityEngine;
using System.Collections;

public class Booster : MonoBehaviour {
    public Vector3 offset, rotationVelocity;
    public float recycleOffset, spawnChance;

    int bType;
    public Material mCalo;
    public Material m2Jump;
	// Use this for initialization
	void Start () {
        GameEventManager.GameOver += GameOver;
        GameEventManager.GamePause += GamePause;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localPosition.x + recycleOffset < Runner.distanceTraveled)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.Rotate(rotationVelocity * Time.deltaTime);
	}

    public void SpawnIfAvailable(Vector3 position, int type)
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        bType = type;
        switch (type)
        {
            case 1:
                GetComponent<Renderer>().material = mCalo;
                break;
            case 2:
                GetComponent<Renderer>().material = m2Jump;
                break;
            default:
                break;
        }

        transform.localPosition = position + offset;
        gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameObject.SetActive(false);
    }

    private void GamePause()
    {
        
    }
    void OnTriggerEnter()
    {
        switch (bType)
        {
            case 1:
                Runner.AddCalo();
                break;
            case 2:
                Runner.AddBoost();
                break;
            default:
                break;
        }
        
        gameObject.SetActive(false);
    }
}
