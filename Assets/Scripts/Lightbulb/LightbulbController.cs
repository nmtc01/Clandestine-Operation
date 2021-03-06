using System.Collections;
using UnityEngine;

public class LightbulbController : MonoBehaviour
{
    [SerializeField]
    private Lightbulb lightbulb = null;

    private void Start()
    {
        lightbulb.SetController(this);
    }

    public void ShatteredLightbulb()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(Random.Range(2f, 3f));

        lightbulb.Spawn();

        yield return null;
    }
}
