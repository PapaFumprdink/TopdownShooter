using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Damage;
    [SerializeField] private Vector2 m_MuzzleVelocity;
    [SerializeField] private float m_Lifetime;

    private Rigidbody2D m_Rigidbody;
    private float m_Age;

    public GameObject Shooter { get; set; }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_Rigidbody.velocity = transform.rotation * m_MuzzleVelocity;
    }

    private void Update()
    {
        m_Age += Time.deltaTime;

        if (m_Age > m_Lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage(m_Damage, Shooter, m_Rigidbody.position, m_Rigidbody.velocity.normalized);
        }

        Destroy(gameObject);
    }
}
