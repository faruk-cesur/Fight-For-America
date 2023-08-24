using UnityEngine;
using UnityEngine.UI;

public interface IShootable
{
    public Health ShootableHealth { get; set; }
    public ParticleSystem ShootableParticle { get; set; }
    public Collider ShootableCollider { get; set; }
    public Rigidbody ShootableRigidbody { get; set; }
    public Slider ShootableSlider { get; set; }
    void GetShot(float damage);
    void Death();
}