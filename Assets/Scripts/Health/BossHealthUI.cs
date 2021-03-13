using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : HealthUIController
{
    [SerializeField]
    private Slider secondSlider = null; 

    public override void SetMaxValue(float maxValue)
    {
        base.SetMaxValue(maxValue);

        secondSlider.maxValue = maxValue;
    }

    public override void SetValue(float value)
    {
        base.SetValue(value);

        secondSlider.value = value;
    }
}
