using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShootWithMouse : MonoBehaviour
{
    [SerializeField] private bool DoLog = false;
    [SerializeField] private Vector3 ShootLocation;
    [Header("Bullet Object")]
    [SerializeField] private bool UseBulletObject=true;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float speed=100;
    [SerializeField] private float size=0.1f;
    [SerializeField] private float lifespan = 3f;
    [Header("Bullet Damage")]
    [SerializeField] private bool doDamage;
    public bool doShoot;
    public int Damage = 100;
    public int CritRate = 10;
    [Header("Bullet Effect")]
    [SerializeField] private GameObject BulletHitEffect;


    public void Shoot()
    {
        if (doShoot)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (UseBulletObject)
                {
                    if (BulletPrefab == null)
                    {
                        BulletPrefab = new("bullet");
                        BulletPrefab.transform.position = transform.position + ShootLocation;
                        BulletPrefab.AddComponent<Rigidbody>();
                        BulletPrefab.GetComponent<Rigidbody>().useGravity = false;
                        BulletPrefab.GetComponent<Rigidbody>().mass = 10;
                        BulletPrefab.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
                        BulletPrefab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                        BulletPrefab.AddComponent<SphereCollider>();
                        BulletPrefab.GetComponent<SphereCollider>().radius = size;
                        Vector3 direction = (hit.point - transform.position).normalized;
                        BulletPrefab.GetComponent<Rigidbody>().linearVelocity = direction * speed;
                        BulletPrefab.AddComponent<TrailRenderer>();
                        BulletPrefab.GetComponent<TrailRenderer>().time = size;
                        BulletPrefab.GetComponent<TrailRenderer>().startWidth = size;
                        BulletPrefab.GetComponent<TrailRenderer>().endWidth = size;
                        BulletPrefab.AddComponent<CollisionDetection>();
                        Coroutine objectdetect = StartCoroutine(DetectObject(BulletPrefab));
                        StartCoroutine(TimedDeath(BulletPrefab, objectdetect));
                    }
                    else
                    {
                        GameObject bullet = Instantiate(BulletPrefab, transform.position + ShootLocation, Quaternion.identity);
                        Vector3 direction = (hit.point - transform.position).normalized;
                        bullet.GetComponent<Rigidbody>().linearVelocity = direction * speed;
                        Coroutine objectdetect = StartCoroutine(DetectObject(bullet));
                        StartCoroutine(TimedDeath(bullet, objectdetect));
                    }
                }
                else
                {
                    if (BulletHitEffect != null)
                    {
                        Instantiate(BulletHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    if (DoLog) Debug.Log("Shot " + hit.transform.name);
                    if (doDamage)
                    {
                        int critChance = Random.Range(0, 101);
                        if (critChance <= CritRate)
                        {
                            Debug.Log("Critical Hit!!!");
                            SendDamage(hit.transform.gameObject, Damage * 2);
                        }
                        else
                        {
                            SendDamage(hit.transform.gameObject, Damage);
                        }
                    }
                }
            }
        }
    }

    private void SendDamage(GameObject obj, int dmg)
    {
        try
        {
            obj.GetComponent<StatsSystem>().TakeDamage(dmg);
        }
        catch
        {
            if (DoLog) Debug.Log("No Health Component found on " + obj.name);
        }
    }

    private IEnumerator TimedDeath(GameObject obj, Coroutine coroutine)
    {
        yield return new WaitForSeconds(lifespan);
        StopCoroutine(coroutine);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    private IEnumerator DetectObject(GameObject obj)
    {
        if (doDamage)
        {
            do
            {
                if (obj.GetComponent<CollisionDetection>().collided)
                {
                    SendDamage(obj.GetComponent<CollisionDetection>().DetectedObject, Damage);
                }
                yield return null;
            }
            while (obj.GetComponent<CollisionDetection>().DetectedObject == null);
        }
    }
}
