using UnityEngine;

public class RedLight : MonoBehaviour
{
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerCountDown.IsCounting()) 
        {
            myLight.color = Color.Lerp(myLight.color, Color.red*3, Time.deltaTime);
        }
        else
        {
            myLight.color = Color.white;
        }
    }
}
