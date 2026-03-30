using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] int DamageAmount = 100;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] float activationDistance = 30f;
    [SerializeField] bool doMove = true;    

    [Header("Loot Table")]
    [SerializeField] ItemClass[] possibleLoot;
    [SerializeField] GameObject Item;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (doMove)
        {
            if (target != null)
            {
                if (Vector3.Distance(agent.transform.position, target.position) <= activationDistance)
                {
                    agent.SetDestination(target.position);
                }
            }
        }
    }

    private void OnDestroy()
    {
        int amount = Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            float x = Random.Range(0, 2);
            float y = Random.Range(0, 2);
            float z = Random.Range(0, 2);
            Vector3 spawnoffset = new(x, y+1, z);
            GameObject droppedItem = Instantiate(Item, transform.position+spawnoffset, Quaternion.identity);
            droppedItem.GetComponent<ItemScript>().item = possibleLoot[Random.Range(0, possibleLoot.Length)];
            GameManager.Instance.EnemiesAlive.Remove(gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<StatsSystem>().TakeDamage(DamageAmount);
        }
    }
}
