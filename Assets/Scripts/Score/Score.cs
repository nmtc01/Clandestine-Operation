using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    #region Singleton
    private static Score instance = null;

    public static Score GetInstance()
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

    private TMP_Text score_value_text;
    private float score_value = 0f;
    private bool canIncreaseScore = true;

    private void Start()
    {
        score_value_text = GetComponent<TMP_Text>();
        UpdateScoreText();
    }

    public static void IncreaseScore(float inc)
    {
        instance.IncreaseInstanceScore(inc);
    }

    public static float GetFinalScore()
    {
        instance.canIncreaseScore = false;
        return instance.score_value;
    }

    private void IncreaseInstanceScore(float inc)
    {
        if (!canIncreaseScore) return;

        score_value += inc;

        if (score_value < 0) score_value = 0;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        score_value_text.text = score_value.ToString();
    }
}
