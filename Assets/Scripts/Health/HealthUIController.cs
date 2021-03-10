using UnityEngine;
using UnityEngine.UI;

public abstract class HealthUIController : MonoBehaviour
{
    [SerializeField]
    private Slider slider = null;

    public virtual void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
    }

    public float GetValue()
    {
        return slider.value;
    }

    public virtual void SetValue(float value)
    {
        slider.value = value;
    }
}
