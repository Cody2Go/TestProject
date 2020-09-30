using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;

    Transform player;
    public float playerDistance;

    Transform[] allies;
    Transform target;

    NavMeshAgent agent;

    CharacterCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;

        allies = new Transform[AllyManager.instance.allies.Length];
        for (int i = 0; i < AllyManager.instance.allies.Length; i++)
        {
            allies[i] = AllyManager.instance.allies[i].transform;
        }

        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetAll();
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void TargetOnlyPlayer()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                CharacterStats targetStats = target.GetComponent<CharacterStats>();

                if (targetStats != null)
                {
                    combat.Attack(targetStats);
                }

                FaceTarget();
            }
        }
    }

    void TargetAll()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);

        if (playerDistance <= lookRadius)
        {
            target = player.transform;
            agent.SetDestination(target.position);

            if (playerDistance <= agent.stoppingDistance)
            {
                CharacterStats targetStats = player.GetComponent<CharacterStats>();

                if (targetStats != null)
                {
                    combat.Attack(targetStats);
                }

                FaceTarget();
            }
        }

        foreach (Transform ally in allies)
        {
            if (ally != null)
            {
                float allyDistance = Vector3.Distance(ally.position, transform.position);

                if (allyDistance <= lookRadius)
                {
                    target = ally.transform;
                    agent.SetDestination(target.position);

                    if (allyDistance <= agent.stoppingDistance)
                    {
                        CharacterStats targetStats = ally.GetComponent<CharacterStats>();

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
