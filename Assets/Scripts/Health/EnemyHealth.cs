public class EnemyHealth : Health
{
    protected override void Start()
    {
        base.Start();

        currentHealth = maxHealth;
        healthUIController.SetValue(currentHealth);
        healthUIController.gameObject.SetActive(false);
    }

    public override void Kill()
    {
        base.Kill();

        Destroy(healthUIController.gameObject);
    }

    public void ShowUI()
    {
        healthUIController.gameObject.SetActive(true);
    }
}
