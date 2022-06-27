using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RedLight : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume = null;

    private ColorGrading colorGrading;

    private void Start()
    {
        TimerCountDown.GetInstance().AddRedLight(this);
        colorGrading = volume.profile.GetSetting<ColorGrading>();
    }


    public void ActivateRedLight(bool active)
        => colorGrading.mixerRedOutRedIn.Override(active ? 200 : 100);


    private void OnDestroy() => TimerCountDown.GetInstance()?.RemoveRedLight(this);
}
