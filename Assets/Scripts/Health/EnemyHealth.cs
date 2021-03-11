public class EnemyHealth : Health
{
    protected override void Start()
    {
        base.Start();

        currentHealth = maxHealth;
        healthUIController.SetValue(currentHealth);
    }


    public override void Kill()
    {
        base.Kill();

        Destroy(healthUIController.gameObject);
    }
}
