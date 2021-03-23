using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    #region Singleton
    private static GameWin instance = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            instance = this;
        }
        instance.gameObject.SetActive(false);
    }
    #endregion

    [SerializeField]
    private TMP_Text finalScoreText = null;
    [SerializeField]
    private GameObject canvas = null;

    public static void RestartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public static void MainMenu()
    {
        GameManager.DestroyObject();
        SceneManager.LoadScene("Menu");
    }

    public static void ShowGameWinScreen()
    {
        instance.gameObject.SetActive(true);
        Player.GetInstanceMovement().enabled = false;
        Player.GetInstanceShoot().enabled = false;
        Player.GetInstanceHealth().enabled = false;
        instance.UpdateFinalScore();

        Destroy(instance.canvas);
        
        AudioSource audioSource = instance.GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Play();
        }
    }

    private void UpdateFinalScore()
    {
        finalScoreText.text = Score.GetFinalScore().ToString();
    }
}
