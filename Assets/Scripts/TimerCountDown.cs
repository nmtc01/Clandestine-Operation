using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    public GameObject textDisplay;
    public bool takingAway = false;
    public bool startCounting = false;

    const int time = 30;
    public int secondsLeft;

    void Start()
    {
        secondsLeft = time;
        textDisplay.GetComponent<Text>().text = "00:" + secondsLeft;
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
        if (!startCounting)
        {
            startCounting = true;
            secondsLeft = time;
        }
    }

    public void StopCounting()
    {
        if (startCounting)
        {
            startCounting = false;
            secondsLeft = time;
        }
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
}
