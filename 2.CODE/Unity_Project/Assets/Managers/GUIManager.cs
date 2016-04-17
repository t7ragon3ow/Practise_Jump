using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class GUIManager : MonoBehaviour
{
    public GUIText boostsText, distanceText;
    public GameObject mainMenu, btnPlay;
    private static GUIManager instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        boostsText.enabled = false;
        distanceText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TriggerGameStart()
    {
        GameEventManager.TriggerGameStart();
    }
    private void GameStart()
    {
        boostsText.enabled = true;
        distanceText.enabled = true;

        instance.mainMenu.SetActive(false);
        enabled = false;
    }

    private void GameOver()
    {
        boostsText.enabled = false;
        distanceText.enabled = false;
        instance.mainMenu.SetActive(true);
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

    public void showAds()
    {
        StartCoroutine(ShowAdWhenReady());
    }

    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady())
            yield return null;

        Advertisement.Show();
    }
}
