using UnityEngine;

public class Player : MonoBehaviour
{
    private static GameObject instance = null;
    private static PlayerControl control = null;
    private static PlayerMovement movement = null;
    private static PlayerShoot shoot = null;
    private static PlayerHealth health = null;

    public static GameObject GetInstance()
    {
        return instance;
    }

    public static PlayerControl GetInstanceControl()
    {
        return control;
    }

    public static PlayerMovement GetInstanceMovement()
    {
        return movement;
    }

    public static PlayerShoot GetInstanceShoot()
    {
        return shoot;
    }

    public static PlayerHealth GetInstanceHealth()
    {
        return health;
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
            movement = instance.GetComponent<PlayerMovement>();
            shoot = instance.GetComponent<PlayerShoot>();
            health = instance.GetComponent<PlayerHealth>();
        }
    }
}
