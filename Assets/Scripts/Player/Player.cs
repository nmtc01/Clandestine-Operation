using UnityEngine;

public class Player : MonoBehaviour
{
    private static GameObject instance = null;
    private static PlayerControl control = null;

    public static GameObject GetInstance()
    {
        return instance;
    }

    public static PlayerControl GetInstanceControl()
    {
        return control;
    }

    private void Awake()
    {
        if(instance != null && instance != gameObject)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = gameObject;
            control = instance.GetComponent<PlayerControl>();
        }
    }
}
