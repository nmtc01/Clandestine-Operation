using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    public GameObject textDisplay;
    public bool takingAway = false;
    private bool startCounting = false;

    const int time = 30;
    public static int secondsLeft;

    public static int enemiesAlerted = 0;

    void Start()
    {
        secondsLeft = time;
        textDisplay.GetComponent<Text>().text = "";
    }

    void Update()
    {
        if (takingAway == false && secondsLeft > 0 && startCounting == true)
        {
            StartCoroutine(TimerTake());
        }
    }

    public void StartCounting()
    {
        startCounting = true;
        secondsLeft = time;
    }

    public void StopCounting()
    {
        startCounting = false;
        secondsLeft = time;
        textDisplay.GetComponent<Text>().text = "";
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft < 10)
            textDisplay.GetComponent<Text>().text = "00:0" + secondsLeft;
        else textDisplay.GetComponent<Text>().text = "00:" + secondsLeft;
        takingAway = false;
    }

    public bool isFinished() 
    {
        return secondsLeft <= 0;
    }

    public void incrementEnemiesAlerted()
    {
        enemiesAlerted++;
    }

    public void decrementEnemiesAlerted()
    {
        enemiesAlerted--;
        if (enemiesAlerted == 0)
            StopCounting();
    }
}
