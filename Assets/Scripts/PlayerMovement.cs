using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;    
    [SerializeField] private float m_AccelerationTime;

    private PlayerInput m_Input;
    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Input = GetComponent<PlayerInput>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector2 currentVelocity = m_Rigidbody.velocity;
        Vector2 targetVelocity = m_Input.MovementDirection * m_MovementSpeed;

        Vector2 force = Vector2.ClampMagnitude(targetVelocity - currentVelocity, m_MovementSpeed) / m_AccelerationTime;
        m_Rigidbody.velocity += force * Time.deltaTime;
    }
}
