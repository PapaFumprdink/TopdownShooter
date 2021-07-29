using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class VitalityHUD : MonoBehaviour
{
    [SerializeField] private Health m_Target;
    [SerializeField] private Transform m_SliderFill;
    [SerializeField] private float m_MinScale;
    [SerializeField] private TMPro.TMP_Text m_DisplayText;
    [SerializeField] private string m_DisplayTextTemplate;

    private void Update()
    {
        float normalizedHealth = m_Target.CurrentHealth / m_Target.MaxHealth;

        m_SliderFill.transform.localScale = new Vector3(normalizedHealth * (1f - m_MinScale) + m_MinScale, 1f, 1f);

        m_DisplayText.text = string.Format(m_DisplayTextTemplate, m_Target.CurrentHealth, m_Target.MaxHealth);
    }
}
