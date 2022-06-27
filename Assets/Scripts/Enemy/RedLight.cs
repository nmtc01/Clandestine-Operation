using UnityEngine;

public class RedLight : MonoBehaviour
{
    private Light myLight;

    private void Start() => myLight = GetComponent<Light>();

    // Update is called once per frame
    private void Update()
    {
        myLight.color = TimerCountDown.IsCounting() ? 
            Color.Lerp(myLight.color, Color.red*3, Time.deltaTime)
            : Color.white;
    }
}
