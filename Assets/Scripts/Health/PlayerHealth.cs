using UnityEngine;

public class PlayerHealth : Health
{
    private bool playedDeathSound = false;

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

        if (!playedDeathSound)
        {
            playedDeathSound = true;
            GetComponent<AudioSource>().Play();        
        }
    }
}
