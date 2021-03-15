using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance = null;
    private static PlayerControl control = null;
    private static PlayerMovement movement = null;
    private static PlayerShoot shoot = null;
    private static PlayerHealth health = null;
    private static Rigidbody rb = null;

    [SerializeField]
    private Transform armature = null;

    public static GameObject GetInstance()
    {
        return instance.gameObject;
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

    public static Rigidbody GetInstanceRigidbody()
    {
        return rb;
    }

    public static Transform GetArmatureTransform()
    {
        return instance.armature;
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            control = instance.GetComponent<PlayerControl>();
            movement = instance.GetComponent<PlayerMovement>();
            shoot = instance.GetComponent<PlayerShoot>();
            health = instance.GetComponent<PlayerHealth>();
            rb = instance.GetComponent<Rigidbody>();
        }
    }

    public static void EnablePhysics(bool enable = true)
    {
        rb.useGravity = enable;
        rb.detectCollisions = enable;
    }
}
