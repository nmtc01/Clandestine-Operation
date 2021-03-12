using UnityEngine;

public class PlayerHealth : Health
{
    private bool playedDeathSound = false;

    [SerializeField]
    private string deathClipPath = "", healClipPath = "";
    private AudioSource audioSource;

    protected override void Start()
    {
        healthUIController = PlayerHealthUI.instance;
        currentHealth = healthUIController.GetValue();

        audioSource = GetComponent<AudioSource>();

        base.Start();
    }

    public override void Kill()
    {
        base.Kill();
        
        GameOver.ShowGameOverScreen();

        if (!playedDeathSound)
        {
            playedDeathSound = true;

            AudioClip deathClip = Resources.Load<AudioClip>(deathClipPath);
            PlayClip(deathClip);
        }
    }

    public override void Heal(float value)
    {
        base.Heal(value);

        AudioClip healClip = Resources.Load<AudioClip>(healClipPath);
        PlayClip(healClip);
    }

    private void PlayClip(AudioClip ac)
    {
        audioSource.clip = ac;
        audioSource.Play();

    }
}
