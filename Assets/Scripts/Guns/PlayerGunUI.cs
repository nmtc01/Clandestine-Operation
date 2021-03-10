using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunUI : MonoBehaviour
{
    #region Singleton
    public static PlayerGunUI instance;

    private void Awake()
    {
        if (instance != null && instance != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private TMP_Text clipMaxSize = null, clipCurrentSize = null;
    [SerializeField]
    private GameObject infinitySymbol = null;
    [SerializeField]
    private Image image = null;

    public void InitSlider(int maxValue, int currentValue, bool infSymb = false)
    {
        slider.maxValue = maxValue;
        SetSliderValue(currentValue);
        infinitySymbol.SetActive(infSymb);
    }

    public void InitSlider()
    {
        InitSlider(1, 1, true);
    }

    public void SetSliderValue(int currentValue)
    {
        slider.value = currentValue;
    }

    public void SetClipProperties(int maxSize, int currentSize)
    {
        clipMaxSize.text = maxSize.ToString();
        SetClipCurrentSize(currentSize);
    }

    public void SetClipCurrentSize(int currentSize)
    {
        clipCurrentSize.text = currentSize.ToString();
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
