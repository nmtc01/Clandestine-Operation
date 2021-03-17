using UnityEngine;

public class FourthWall : MonoBehaviour
{
    private static FourthWall instance = null;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            instance.gameObject.SetActive(false);
        }
    }

    public static GameObject GetInstance()
    {
        if (instance) return instance.gameObject;
        return null;
    }

}
