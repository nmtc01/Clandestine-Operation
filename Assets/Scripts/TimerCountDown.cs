using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerCountDown : MonoBehaviour
{
    #region Singleton
    private static TimerCountDown instance = null;

    public static TimerCountDown GetInstance()
    {
        return instance;
    }

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
    }
    #endregion

    private TMP_Text textDisplay;
    private bool startCounting = false;

    const int time = 30;
    public static int secondsLeft;

    public static int enemiesAlerted = 0;

    private IEnumerator timer = null;

    void Start()
    {
        textDisplay = GetComponent<TMP_Text>();

        secondsLeft = time;
        textDisplay.text = "";
    }

    void Update()
    {
        if (startCounting && timer == null)//!takingAway && secondsLeft > 0 && startCounting)
        {
            timer = TimerTake();
            StartCoroutine(timer);
        }
    }

    public static void StartCounting()
    {
        instance.SetCounting(true);
    }

    public static void StopCounting()
    {
        instance.SetCounting(false);
    }

    private void SetCounting(bool count)
    {
        startCounting = count;
        secondsLeft = time;
    }

    private IEnumerator TimerTake()
    {
        while(secondsLeft > 0) {
            
            UpdateText();

            yield return new WaitForSeconds(1);
            secondsLeft -= 1;
        }

        UpdateText();
        yield return null;
    }
    private void UpdateText()
    {
        if (secondsLeft < 10)
            textDisplay.text = "00:0" + secondsLeft;
        else 
            textDisplay.text = "00:" + secondsLeft;
    }

    public static bool IsFinished() 
    {
        return secondsLeft <= 0;
    }

    private bool GetCounting()
    {
        return startCounting;
    }

    public static bool IsCounting()
    {
        return instance.GetCounting();
    }

    public static void IncrementEnemiesAlerted()
    {
        enemiesAlerted++;
    }

    public static void DecrementEnemiesAlerted()
    {
        enemiesAlerted--;
        if (enemiesAlerted == 0)
        {
            instance.StopTimer();
            StopCounting();
        }
    }

    private void StopTimer()
    {
        StopCoroutine(timer);
        timer = null;
        textDisplay.text = "";
    }
}
