using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
    public GUIText boostsText, distanceText, gameOverText, instructionsText, runnerText;
    private static GUIManager instance;
	// Use this for initialization
	void Start () {
        instance = this;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        gameOverText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            GameEventManager.TriggerGameStart();
        }
	}

    private void GameStart()
    {
        gameOverText.enabled = false;
        instructionsText.enabled = false;
        runnerText.enabled = false;
        enabled = false;
    }

    private void GameOver()
    {
        gameOverText.enabled = true;
        instructionsText.enabled = true;
        enabled = true;
    }

    public static void SetBoosts(int boosts)
    {
        instance.boostsText.text = boosts.ToString();
    }

    public static void SetDistance(float distance)
    {
        instance.distanceText.text = distance.ToString("f0");
    }
}
