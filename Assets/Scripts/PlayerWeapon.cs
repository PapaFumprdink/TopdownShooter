using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private int m_ProjectilesPerShot;
    [SerializeField] private float m_Spray;

    [Space]
    [SerializeField] private float m_AutoFirerate;
    [SerializeField] private float m_SingleFirerate;
    
    [Space]
    [SerializeField] private int m_MagazineSize;
    [SerializeField] private float m_ReloadTime;

    [Space]
    [SerializeField] private GameObject m_Shooter;
    [SerializeField] private Transform m_Muzzle;

    private PlayerInput m_Input;
    private float m_LastFireTime;
    private int m_CurrentMagazine;
    private bool m_IsReloading;

    public int MagazineSize => m_MagazineSize;
    public int CurrentMagazine => m_CurrentMagazine;

    private void Update()
    {
        if (!m_Input)
        {
            m_Input = GetComponentInParent<PlayerInput>();
        }

        bool canFire = (m_Input.FiredThisFrame && Time.time > m_LastFireTime + 60f / m_SingleFirerate)
                       || (m_Input.IsFiring && Time.time > m_LastFireTime + 60f / m_AutoFirerate);

        if (canFire)
        {
            if (m_CurrentMagazine > 0)
            {
                for (int i = 0; i < m_ProjectilesPerShot; i++)
                {
                    float spray = (i / (m_ProjectilesPerShot - 1f)) * m_Spray - m_Spray / 2f;
                    GameObject projectileInstance = Instantiate(m_ProjectilePrefab, m_Muzzle.position, m_Muzzle.rotation * Quaternion.Euler(0f, 0f, spray));
                    
                    if (projectileInstance.TryGetComponent(out Projectile projectile))
                    {
                        projectile.Shooter = m_Shooter;
                    }
                }

                m_CurrentMagazine--;
                m_LastFireTime = Time.time;
            }
            else
            {
                StartCoroutine(Reload());
            }
        }

        if (m_Input.IsReloading && m_CurrentMagazine < m_MagazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if (m_IsReloading) yield break;

        m_IsReloading = true;
        m_CurrentMagazine = 0;

        yield return new WaitForSeconds(m_ReloadTime);

        m_CurrentMagazine = m_MagazineSize;
        m_IsReloading = false;
    }
}
