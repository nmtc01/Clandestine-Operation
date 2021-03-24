using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    #region Singleton
    private static GameOver instance = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        instance.gameObject.SetActive(false);
    }
    #endregion

    [SerializeField]
    private TMP_Text finalScoreText = null;

    public static void RestartLevel(string levelName)
    {
        ResetTimer();
        DestroyObject();
        SceneManager.LoadScene(levelName);
    }

    public static void MainMenu()
    {
        ResetTimer();
        GameManager.DestroyObject();
        DestroyObject();
        SceneManager.LoadScene("Menu");
    }

    private static void ResetTimer()
    {
        instance.StopAllCoroutines();
        Time.timeScale = 1f;
        TimerCountDown.ResetEnemiesAlerted();
    }

    public static void ShowGameOverScreen()
    {
        instance.gameObject.SetActive(true);
        Player.GetInstanceMovement().enabled = false;
        Player.GetInstanceShoot().enabled = false;
        instance.UpdateFinalScore();

        instance.StartCoroutine(instance.Stop());
    }

    private void UpdateFinalScore()
    {
        finalScoreText.text = Score.GetFinalScore().ToString();
    }

    private IEnumerator Stop()
    {
        float stopingTime = 2f;
        for (float t = 0f; t <= stopingTime; t += Time.deltaTime)
        {
            Time.timeScale = Mathf.Lerp(1f, 0f, t / stopingTime);
            yield return null;
        }
    }

    public static void DestroyObject()
    {
        Destroy(instance.transform.parent.gameObject);
    }
}
