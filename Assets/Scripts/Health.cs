using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_CurrentHealth;

    public float MaxHealth => m_MaxHealth;
    public float CurrentHealth => m_CurrentHealth;

    public void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction)
    {
        m_CurrentHealth -= damage;

        if (m_CurrentHealth < 0)
        {
            Kill(damage, damager, point, direction);
        }
    }

    public void DamageOverTime(float totalDamage, float time, GameObject damager, Vector2 point, Vector2 direction)
    {
        StartCoroutine(DamageOverTimeRoutine(totalDamage, time, damager, point, direction));
    }

    private IEnumerator DamageOverTimeRoutine(float totalDamage, float time, GameObject damager, Vector2 point, Vector2 direction)
    {
        float timeDamaged = 0f;
        while (timeDamaged < time)
        {
            m_CurrentHealth -= totalDamage / time * Time.deltaTime;

            timeDamaged += Time.deltaTime;
            yield return null;
        }
    }

    public void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction)
    {
        gameObject.SetActive(false);
    }
}
