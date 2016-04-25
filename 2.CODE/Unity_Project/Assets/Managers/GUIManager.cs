using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class GUIManager : MonoBehaviour
{
    public GUIText boostsText, distanceText, caloText;

    public TextEditor distanceValue;
    public GameObject mainMenu, btnPlay, result;
    private static GUIManager instance;
    public Runner runner;
    // Use this for initialization
    void Start()
    {
        instance = this;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        GameEventManager.GamePause += GamePause;
        GameEventManager.GameResume += GameResume;
        boostsText.enabled = false;
        distanceText.enabled = false;
        caloText.enabled = false;

        instance.mainMenu.SetActive(true);
        instance.result.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GameResume()
    {
        Time.timeScale = 1;

        boostsText.enabled = true;
        distanceText.enabled = true;
        caloText.enabled = true;

        instance.result.SetActive(false);
        instance.mainMenu.SetActive(false);
        enabled = false;

        if( runner.transform.position.y <= runner.gameOverY)
        {
            runner.transform.localPosition = new Vector3(runner.transform.localPosition.x, runner.transform.localPosition.y - runner.gameOverY + 5, runner.transform.localPosition.z);
         
        }
    }
    private void GamePause()
    {
        Time.timeScale = 0;

        boostsText.enabled = false;
        distanceText.enabled = false;
        caloText.enabled = false;

        instance.result.SetActive(true);
        instance.mainMenu.SetActive(false);
        enabled = true;

        //setResult(Runner.distanceTraveled);
    }

    public void TriggerGameStart()
    {
        GameEventManager.TriggerGameStart();
    }
    private void GameStart()
    {
        Time.timeScale = 1;

        boostsText.enabled = true;
        distanceText.enabled = true;
        caloText.enabled = true;

        instance.mainMenu.SetActive(false);
        instance.result.SetActive(false);
        enabled = false;
    }

    private void GameOver()
    {
        boostsText.enabled = false;
        distanceText.enabled = false;
        caloText.enabled = false;

        instance.mainMenu.SetActive(true);
        instance.result.SetActive(false);
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

    public static void SetCalo(int calo)
    {
        instance.caloText.text = calo.ToString();
    }

    public static void setResult( float distance)
    {
        //instance.distanceValue.text = distance.ToString();
    }

    public void resumeGame()
    {
        GameResume();
    }
    public void showAds()
    {
        StartCoroutine(ShowAdWhenReady());
    }

    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady())
            yield return null;

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        Advertisement.Show("", options);
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                GameResume();
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }
}
