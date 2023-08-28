using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform TargetTransform { get; set; }
    public Enemy TargetEnemy { get; set; }
    public float BulletDamage { get; set; }
    public ObjectPool BulletObjectPool;
    [SerializeField] private float _bulletSpeed;

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        var myPosition = transform.position;
        transform.position = Vector3.MoveTowards(myPosition, TargetPositionOffsetY(), Time.deltaTime * _bulletSpeed);
        transform.LookAt(TargetPositionOffsetY());

        if (IsBulletReachedTarget())
        {
            TargetEnemy.GetShot(BulletDamage);
            BulletObjectPool.SetPooledObject(gameObject,0);
        }
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