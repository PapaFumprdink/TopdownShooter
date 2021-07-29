using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponUI : MonoBehaviour
{
    [SerializeField] private WeaponManager m_WeaponManager;
    [SerializeField] private Transform m_AmmoIndicator;

    [Space]
    [SerializeField] private float m_ShotScaleReduction;
    [SerializeField] private float m_ShotScaleSmoothing;

    private List<Transform> m_AmmoIndicatorList;

    private void Awake()
    {
        m_AmmoIndicatorList = new List<Transform>();
        m_AmmoIndicatorList.Add(m_AmmoIndicator);
    }

    private void Update()
    {
        Application.targetFrameRate = Keyboard.current.rightBracketKey.isPressed ? 30 : -1;

        int currentMagazine = 0;
        int magazineSize = 0;

        GameObject currentWeaponObject = m_WeaponManager.CurrentWeapon;
        PlayerWeapon currentWeapon = null;

        if (currentWeaponObject ? currentWeaponObject.TryGetComponent(out currentWeapon) : false)
        {
            currentMagazine = currentWeapon.CurrentMagazine;
            magazineSize = currentWeapon.MagazineSize;
        }

        for (int i = 0; i < currentMagazine - m_AmmoIndicatorList.Count; i++)
        {
            m_AmmoIndicatorList.Add(Instantiate(m_AmmoIndicator, m_AmmoIndicator.parent));
        }

        for (int i = 0; i < m_AmmoIndicatorList.Count; i++)
        {
            if (i < magazineSize)
            {
                Vector3 targetScale = Vector3.one * (i < currentMagazine ? 1f : 1f / m_ShotScaleReduction);
                m_AmmoIndicatorList[i].localScale += (targetScale - m_AmmoIndicatorList[i].localScale) * m_ShotScaleSmoothing * Time.deltaTime;
                m_AmmoIndicatorList[i].gameObject.SetActive(true);
            }
            else
            {
                m_AmmoIndicatorList[i].gameObject.SetActive(false);
            }
        }
    }
}
