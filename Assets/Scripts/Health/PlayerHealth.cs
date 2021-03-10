public class PlayerHealth : Health
{
    protected override void Start()
    {
        base.Start();

        PlayerHealthUI.instance.SetMaxValue(maxHealth);
        currentHealth = PlayerHealthUI.instance.GetValue();
    }

    public override void Kill()
    {
        base.Kill();
        SetHealth();

        GameOver.ShowGameOverScreen();
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
        PlayerHealthUI.instance.SetValue(currentHealth);
    }
}
