using UnityEngine;

public class Player : MonoBehaviour
{
    private static GameObject instance = null;

    public static GameObject GetInstance()
    {
        return instance;
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
        }
    }
}
