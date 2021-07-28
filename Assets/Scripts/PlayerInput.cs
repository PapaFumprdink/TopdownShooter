using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool m_UseGamepad;
    [SerializeField] private float m_CursorDistance;

    private PlayerControls m_ControlAsset;

    public Vector2 MovementDirection => m_ControlAsset.Player.Movement.ReadValue<Vector2>();
    public Vector2 LookVector
    {
        get
        {
            if (m_UseGamepad)
            {
                return m_ControlAsset.Player.LookDirection.ReadValue<Vector2>();
            }
            else
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                return mousePos - (Vector2)transform.position;
            }
        }
    }

    private void Awake()
    {
        m_ControlAsset = new PlayerControls();
    }
}
