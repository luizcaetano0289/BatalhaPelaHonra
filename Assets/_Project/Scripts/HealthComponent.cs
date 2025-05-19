using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public Slider healthSlider;

    private UnitStats unitStats;

    public void Initialize(UnitStats stats)
    {
        unitStats = stats;
        unitStats.OnHealthChanged += UpdateHUD;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (unitStats != null && healthSlider != null)
        {
            healthSlider.value = unitStats.GetHealthNormalized();
        }
    }

    private void OnDestroy()
    {
        if (unitStats != null)
            unitStats.OnHealthChanged -= UpdateHUD;
    }
}
