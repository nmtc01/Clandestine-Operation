public class BossHealth : Health
{
    // Extend methods to detect middle of the health bar

    protected override void Start()
    {
        base.Start();

        currentHealth = maxHealth;
        healthUIController.SetValue(currentHealth);
    }
}
