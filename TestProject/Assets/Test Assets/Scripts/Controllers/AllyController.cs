using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public bool isInCombat = false;

    Transform player;
    public float playerDistance;

    Transform[] enemies;
    Transform target;

    NavMeshAgent agent;

    CharacterCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;

        enemies = new Transform[EnemyManager.instance.enemies.Length];
        for (int i = 0; i < EnemyManager.instance.enemies.Length; i++)
        {
            enemies[i] = EnemyManager.instance.enemies[i].transform;
        }

        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetEnemiesFollowPlayer();
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void TargetEnemiesFollowPlayer()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);

        if (playerDistance <= lookRadius)
        {
            target = player.transform;
            agent.SetDestination(target.position);
            isInCombat = false;

            if (playerDistance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

        foreach (Transform enemy in enemies)
        {
            if (enemy != null)
            {
                float enemyDistance = Vector3.Distance(enemy.position, transform.position);

                if (enemyDistance <= lookRadius)
                {
                    target = enemy.transform;
                    agent.SetDestination(target.position);
                    isInCombat = true;

                    if (enemyDistance <= agent.stoppingDistance)
                    {
                        CharacterStats targetStats = enemy.GetComponent<CharacterStats>();

                        if (targetStats != null)
                        {
                            combat.Attack(targetStats);
                        }

                        FaceTarget();
                    }
                }
            }
        }
    }
}
