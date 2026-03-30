
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 target;
    public int damage = 200;
    public float speed = 50f;
    public string TagToDamage;
    public string LayerToAvoidDetection;

    void Start()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.useGravity = false;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        if (target!=null)
        {
            Vector3 direction = (target - transform.position).normalized;
            body.linearVelocity = direction * speed;
        } else
        {
            Debug.LogWarning("Target is not set for the bullet. It will self destruct.");
            Destroy(gameObject);
        }

        gameObject.layer = LayerMask.NameToLayer(LayerToAvoidDetection);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(LayerToAvoidDetection), gameObject.layer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagToDamage))
        {
            if (collision.gameObject.GetComponent<StatsSystem>()!=null)
            {
                collision.gameObject.GetComponent<StatsSystem>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
