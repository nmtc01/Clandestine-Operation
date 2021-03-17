using System.Collections;
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

    private void OnEnable()
    {
        slider.value = 0;
        secondSlider.value = 0;
        StartCoroutine(BossHealthBarAnimation());
    }

    private IEnumerator BossHealthBarAnimation()
    {
        float animTime = 2.5f;
        for (float t = 0; t < animTime; t += Time.deltaTime)
        {
            float currentValue = Mathf.Lerp(0, slider.maxValue, t / animTime);

            slider.value = currentValue;
            secondSlider.value = currentValue;
            yield return null;
        }
    }
}
