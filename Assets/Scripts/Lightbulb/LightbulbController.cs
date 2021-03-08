using System.Collections;
using UnityEngine;

public class LightbulbController : MonoBehaviour
{
    #region Singleton
    private static LightbulbController instance = null;
    public static LightbulbController GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public void ShatteredLightbulb(Lightbulb lightbulb)
    {
        StartCoroutine(Respawn(lightbulb));
    }

    private IEnumerator Respawn(Lightbulb lightbulb)
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));

        lightbulb.Spawn();

        yield return null;
    }
}
