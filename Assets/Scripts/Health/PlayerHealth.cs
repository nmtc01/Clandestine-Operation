using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [SerializeField]
    private Slider slider = null;

    protected override void Start()
    {
        base.Start();

        slider.maxValue = maxHealth;
        SetHealth();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        SetHealth();
    }

    public override void Heal(float value)
    {
        base.Heal(value);
        SetHealth();
    }

    private void SetHealth()
    {
        slider.value = currentHealth;
    }
}
