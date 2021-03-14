using UnityEngine;

public class PlayerHealth : Health
{
    private bool playedDeathSound = false;

    [SerializeField]
    private string deathClipPath = "", healClipPath = "";
    
    [SerializeField]
    private AudioSource mainAudioSource = null, hearthBeatAudioSource = null;

    private static float minHearthBeatVolume = .05f;

    protected override void Start()
    {
        healthUIController = PlayerHealthUI.instance;
        currentHealth = healthUIController.GetValue(); 

        base.Start();

        UpdateHearthBeatAudioSource();
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

            hearthBeatAudioSource.Stop();

            Player.EnablePhysics(false);
            CameraFollow.StopFollowingPlayer();
        }
    }

    public override void Heal(float value)
    {
        base.Heal(value);

        AudioClip healClip = Resources.Load<AudioClip>(healClipPath);
        PlayClip(healClip);

        UpdateHearthBeatAudioSource();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        UpdateHearthBeatAudioSource();
    }

    private void UpdateHearthBeatAudioSource()
    {
        hearthBeatAudioSource.volume = minHearthBeatVolume + (maxHealth - currentHealth) / maxHealth;
        hearthBeatAudioSource.pitch = 1 + (hearthBeatAudioSource.volume - minHearthBeatVolume) / 2f;
    }

    private void PlayClip(AudioClip ac)
    {
        mainAudioSource.clip = ac;
        mainAudioSource.Play();
    }
}
