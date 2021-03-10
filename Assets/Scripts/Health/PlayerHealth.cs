public class PlayerHealth : Health
{
    protected override void Start()
    {
        healthUIController = PlayerHealthUI.instance;
        currentHealth = healthUIController.GetValue();

        base.Start();
    }

    public override void Kill()
    {
        base.Kill();

        GameOver.ShowGameOverScreen();
    }
}
