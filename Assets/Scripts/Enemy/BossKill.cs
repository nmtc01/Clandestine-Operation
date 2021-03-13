using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKill : MonoBehaviour
{
    private float range = 3f;
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Player.GetInstance().transform.position.x - transform.position.x) <= range && Player.GetInstanceControl().IsAlive())
        {
            enemyController.SetKillPlayer(true);
            Player.GetInstanceControl().SetIsCovering(false);
            Player.GetInstanceControl().SetIsFallingBack(true);
        }
        else if (!Player.GetInstanceControl().IsAlive())
        {
            enemyController.SetKillPlayer(false);
        }
    }
}
