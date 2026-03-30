
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float timer = 0;
    [Header("Spawner Properties")]
    public float spawnrate = 10;
    [SerializeField] private GameObject EntityToSpawn;
    [SerializeField] private Transform WhereToSpawn;
    public bool DoSpawn = true;
    public Mode SpawnMode;

    [Header("Gunmode Properties")]
    public string TagToDamage;
    public string LayerToAvoidDetection;
    public bool DoDetection;
    public Transform target;
    public int Damage = 200;
    [SerializeField] private float Speed = 50;
    public float Range = 20;
    [Header("Shotgun Properties")]
    [SerializeField] private float VerticalSpreadAmount = 0.5f;
    [SerializeField] private float HorizontalSpreadAmount = 1.5f;

    [Header("States")]
    public bool Fired;
    public bool FoundTarget;

    //Data
    private float og_range;

    public enum Mode
    {
        Normal,
        GunMode,
        ShotgunMode
    }

    // Optimize SphereCollider setup in Start()
    private void Start()
    {
        if (WhereToSpawn == null)
        {
            WhereToSpawn = transform;
        }

        
    }

    private void Update()
    {
        
        if (SpawnMode != Mode.Normal && DoDetection)
        {
            if (og_range != Range)
            {
                if (!gameObject.TryGetComponent<SphereCollider>(out var sphereCollider))
                {
                    sphereCollider = gameObject.AddComponent<SphereCollider>();
                }
                sphereCollider.radius = Range;
                sphereCollider.isTrigger = true;

                og_range = Range;
            }
        }


        Fired = false;

        //Spawning
        timer += Time.deltaTime;
        if (timer >= spawnrate && DoSpawn)
        {

            try
            {
                if (SpawnMode == Mode.Normal)
                {
                    GameObject spawned = Instantiate(EntityToSpawn, WhereToSpawn.position, Quaternion.identity);
                }
                else if (SpawnMode == Mode.GunMode)
                {
                    GameObject spawned = Instantiate(EntityToSpawn, WhereToSpawn.position, Quaternion.identity);
                    spawned.GetComponent<Bullet>().target = target.position;
                    spawned.GetComponent<Bullet>().damage = Damage;
                    spawned.GetComponent<Bullet>().speed = Speed;
                    spawned.GetComponent<Bullet>().TagToDamage = TagToDamage;
                    spawned.GetComponent<Bullet>().LayerToAvoidDetection = LayerToAvoidDetection;
                }
                else if (SpawnMode == Mode.ShotgunMode)
                {
                    for (int i = 0, count = 20; i < count; i++)
                    {
                        Vector3 randomOffset = new(
                            Random.Range(-HorizontalSpreadAmount, HorizontalSpreadAmount),
                            Random.Range(-VerticalSpreadAmount, VerticalSpreadAmount),
                            Random.Range(-HorizontalSpreadAmount, HorizontalSpreadAmount)
                            );
                        GameObject shot = Instantiate(EntityToSpawn, WhereToSpawn.position, Quaternion.identity);
                        if (shot.TryGetComponent<Bullet>(out var bullet))
                        {
                            bullet.target = target.position;
                            bullet.target += randomOffset;
                            bullet.damage = Damage;
                            bullet.speed = Speed;
                            bullet.TagToDamage = TagToDamage;
                            bullet.LayerToAvoidDetection = LayerToAvoidDetection;
                        }
                    }
                }
                Fired = true;
                timer = 0;
            }
            catch (MissingReferenceException)
            {
                Debug.LogWarning("EntityToSpawn is not set or has been destroyed.");
                target = null;
                FoundTarget = false;
                DoSpawn = false;
            }
            
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (SpawnMode != Mode.Normal && DoDetection)
        {
            if (other.CompareTag(TagToDamage))
            {
                target = other.transform;
                FoundTarget = true;
                DoSpawn = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (SpawnMode != Mode.Normal && DoDetection)
        {
            if (other.CompareTag(TagToDamage))
            {
                target = other.transform;
                FoundTarget = true;
                DoSpawn = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
        DoSpawn = false;
    }

}
