using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerInput : MonoBehaviour
{
    private const float KeyDeadzone = 0.1f;
    [SerializeField] private float m_CursorDistance;

    private PlayerControls m_ControlAsset;

    public Vector2 MovementDirection => m_ControlAsset.Player.Movement.ReadValue<Vector2>();
    public Vector2 LookVector
    {
        get
        {
            if (Gamepad.current != null ? Gamepad.current.wasUpdatedThisFrame : false)
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
    public bool IsFiring => m_ControlAsset.Player.Fire.ReadValue<float>() > KeyDeadzone;
    public bool FiredThisFrame => m_ControlAsset.Player.Fire.triggered;
    public bool IsReloading => m_ControlAsset.Player.Reload.ReadValue<float>() > KeyDeadzone;

    private void Awake()
    {
        m_ControlAsset = new PlayerControls();
    }

    private void OnEnable()
    {
        m_ControlAsset.Enable();
    }

    private void OnDisable()
    {
        m_ControlAsset.Disable();
    }
}
