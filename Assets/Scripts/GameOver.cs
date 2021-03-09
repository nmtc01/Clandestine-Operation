using System.Collections;
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
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        instance.gameObject.SetActive(false);
    }
    #endregion

    public static void RestartLevel(string levelName)
    {
        instance.StopAllCoroutines();
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
    }

    public static void ShowGameOverScreen()
    {
        instance.gameObject.SetActive(true);
        Player.GetInstanceMovement().enabled = false;
        Player.GetInstanceShoot().enabled = false;
        instance.StartCoroutine(instance.Stop());
    }

    private IEnumerator Stop()
    {
        float stopingTime = 2f;
        for(float t = 0f; t <= stopingTime; t += Time.deltaTime)
        {
            Time.timeScale = Mathf.Lerp(1f, 0f, t / stopingTime);
            yield return null;
        }
    }
}
