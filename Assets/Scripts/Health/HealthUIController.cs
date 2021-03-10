using UnityEngine;
using UnityEngine.UI;

public abstract class HealthUIController : MonoBehaviour
{
    [SerializeField]
    private Slider slider = null;

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
    }

    public float GetValue()
    {
        return slider.value;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}
