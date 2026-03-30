
using System.Collections;
using TMPro;
using UnityEngine;

public class FPShooterSystem : MonoBehaviour
{
    [SerializeField] int damage = 500;
    [SerializeField] GameObject bulletHitParticle;
    public int CurCap;
    public int MaxCap = 6;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] int bulletPerShoot = 1;
    [SerializeField] bool semiAuto = true;
    [SerializeField] TextMeshProUGUI CapCounter;
    int prev_CurCap;

    public static FPShooterSystem Instance { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurCap = MaxCap;
    }

    private void Update()
    {
        if (CurCap != prev_CurCap)
        {
            CapCounter.text = CurCap.ToString() + "/" + MaxCap.ToString();
            prev_CurCap = CurCap;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (semiAuto)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                StartCoroutine(AutomaticShoot());
            }
        }
    }

    public void ChangeBullet(ItemBulletClass bullet)
    {
        damage = bullet.damage;
        ReloadingSystem.Instance.itemRequired = bullet;
    }

    IEnumerator Shoot()
    {
        Transform cameraTransform = Camera.main.transform;
        int layerMask = Physics.DefaultRaycastLayers;

        for (int i = bulletPerShoot; i > 0 && CurCap > 0; i--)
        {
            if (CurCap <= 0) break;
            Vector3 rayOrigin = cameraTransform.position;
            Vector3 rayDirection = cameraTransform.forward;

            Debug.DrawRay(rayOrigin, rayDirection * 1000, Color.red);

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit,
                1000, layerMask))
            {
                StatsSystem stats = hit.collider.GetComponent<StatsSystem>();
                if (stats != null)
                {
                    stats.TakeDamage(damage);
                }

                Instantiate(bulletHitParticle, hit.point,
                    Quaternion.LookRotation(hit.normal));
            }

            CurCap--;

            if (i > 1)
            {
                yield return new WaitForSeconds(fireRate / 2);
            }
        }
    }

    IEnumerator AutomaticShoot()
    {
        while (Input.GetMouseButton(0))
        {
            Shoot();
            yield return new WaitForSeconds(fireRate*bulletPerShoot);
        }
    }
}
