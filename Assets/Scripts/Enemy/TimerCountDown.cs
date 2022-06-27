using System.Collections;
using UnityEngine;
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

    private AudioSource audioSource = null;

    private RedLight light;

    private void Start()
    {
        textDisplay = GetComponent<TMP_Text>();
        audioSource = GetComponent<AudioSource>();

        secondsLeft = time;
        textDisplay.text = "";
    }

    private void SetCounting(bool count)
    {
        startCounting = count;
        secondsLeft = time;
        light.ActivateRedLight(count);
    }

    private IEnumerator TimerTake()
    {
        while(secondsLeft > 0) {
            
            UpdateText();

            yield return new WaitForSeconds(1);
            secondsLeft -= 1;
        }

        UpdateText();

        Player.GetInstanceHealth().Kill();

        yield return null;
    }
    private void UpdateText()
    {
        if (secondsLeft < 10)
            textDisplay.text = "00:0" + secondsLeft;
        else 
            textDisplay.text = "00:" + secondsLeft;
    }

    public static bool IsFinished() => secondsLeft <= 0;

    private bool GetCounting() => startCounting;

    public static bool IsCounting() => instance.GetCounting();

    public static void IncrementEnemiesAlerted()
    {
        if (enemiesAlerted == 0) instance.StartTimer();

        enemiesAlerted++;
    }

    public static void DecrementEnemiesAlerted()
    {
        enemiesAlerted--;
        if (enemiesAlerted == 0) instance.StopTimer();
    }

    public static void ResetEnemiesAlerted() => enemiesAlerted = 0;

    private void StopTimer()
    {
        StopCoroutine(timer);
        instance.SetCounting(false);
        timer = null;
        textDisplay.text = "";
        instance.audioSource.Stop();
    }

    private void StartTimer()
    {
        timer = TimerTake();
        StartCoroutine(timer);
        instance.audioSource.Play();
        instance.SetCounting(true);
    }

    public void AddRedLight(RedLight l) => light = l;
    public void RemoveRedLight(RedLight l)
    {
        if (light != l) return;

        light = null;
    }
}