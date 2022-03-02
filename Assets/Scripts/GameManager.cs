using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        Cursor.visible = false;
    }
    #endregion

    public static void DestroyObject()
    {
        Cursor.visible = true;
        Destroy(instance.gameObject);
    }
}