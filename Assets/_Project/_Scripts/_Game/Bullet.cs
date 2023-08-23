using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform TargetTransform { get; set; }
    public Enemy TargetEnemy { get; set; }
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private ParticleSystem _bulletMuzzleParticle;

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        var myPosition = transform.position;
        transform.position = Vector3.MoveTowards(myPosition, TargetPositionOffsetY(), Time.deltaTime * _bulletSpeed);

        if (IsBulletReachedTarget())
        {
            TargetEnemy.EnemyHealth.IsDead = true;
            TargetEnemy.gameObject.layer = 14;
            InstantiateBulletMuzzleParticle();
            Destroy(gameObject);
        }
    }

    private void InstantiateBulletMuzzleParticle()
    {
        var particle = Instantiate(_bulletMuzzleParticle, transform.position, Quaternion.identity);
        Destroy(particle, 2f);
    }

    private Vector3 TargetPositionOffsetY()
    {
        if (TargetTransform)
        {
            return TargetTransform.position + new Vector3(0, TargetTransform.localScale.y, 0);
        }

        return Vector3.zero; //fix this
    }

    private bool IsBulletReachedTarget()
    {
        var bulletPosition = transform.position;
        float distanceToTarget = (TargetPositionOffsetY() - bulletPosition).sqrMagnitude;

        return distanceToTarget * distanceToTarget <= 0f;
    }
}