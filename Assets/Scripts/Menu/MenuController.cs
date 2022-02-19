using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPopUp = null;

    private void Start() => Cursor.visible = true;

    public void ShowTutorialPopUp()
    {
        tutorialPopUp.SetActive(true);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayFirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
