public class CoverHealth : Health
{
    protected override void Start()
    {
        base.Start();

        currentHealth = maxHealth;
        healthUIController.SetValue(currentHealth);
    }

    public override void Damage(float damage)
    {
        if (Player.GetInstanceControl().IsCovering())
            base.Damage(damage);
    }


    public override void Kill()
    {
        base.Kill();

        Destroy(gameObject);
    }
}
