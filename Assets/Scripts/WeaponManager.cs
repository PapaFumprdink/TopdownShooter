using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
public sealed class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform m_LookContainer;
    [SerializeField] private Transform m_WeaponContainer;

    private PlayerInput m_Input;

    public int CurrentWeaponIndex { get; private set; }
    public GameObject CurrentWeapon => m_WeaponContainer.GetChild(CurrentWeaponIndex).gameObject;
        

    private void Awake()
    {
        m_Input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        for (int i = 0; i < m_WeaponContainer.childCount; i++)
        {
            m_WeaponContainer.GetChild(i).gameObject.SetActive(i == 0);
        }
    }

    private void Update()
    {
        m_LookContainer.right = m_Input.LookVector;
    }
}
