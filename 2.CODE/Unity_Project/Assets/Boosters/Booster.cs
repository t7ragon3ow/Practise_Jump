using UnityEngine;
using System.Collections;

public class Booster : MonoBehaviour {
    public Vector3 offset, rotationVelocity;
    public float recycleOffset, spawnChance;
	// Use this for initialization
	void Start () {
        GameEventManager.GameOver += GameOver;
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

    public void SpawnIfAvailable(Vector3 position)
    {
        if (gameObject.activeSelf || spawnChance <= Random.Range(0f, 100f))
        {
            return;
        }
        transform.localPosition = position + offset;
        gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameObject.SetActive(false);
    }
    void OnTriggerEnter()
    {
        Runner.AddBoost();
        gameObject.SetActive(false);
    }
}
