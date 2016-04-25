using UnityEngine;
using System.Collections;

public class Obstacles : MonoBehaviour {
    public Vector3 offset, rotationVelocity;
    public float recycleOffset, spawnChance;
	// Use this for initialization
	void Start () {
        GameEventManager.GameOver += GameOver;
        GameEventManager.GamePause += GamePause;
        gameObject.SetActive(false);
	}

    private void GamePause()
    {
        Time.timeScale = 0;
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
        if (gameObject.activeSelf)
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
        //gameObject.SetActive(false);
        //GameEventManager.TriggerGameOver();
        GameEventManager.TriggerGamePause();
    }
}
