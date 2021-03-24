public class BossHealth : EnemyHealth
{
    private bool firstTime = true;
    public override void Damage(float damage)
    {
        base.Damage(damage);

        if (firstTime && currentHealth <= maxHealth*0.5f)
        {
            firstTime = false;
            BossController.GetInstance().ActivateBossCamera();
        }
    }

    public override void Kill()
    {
        base.Kill();
        BossController.GetInstance().ActivateBossCamera();
    }
}
