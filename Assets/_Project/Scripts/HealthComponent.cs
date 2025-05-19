using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public Slider healthSlider;
    private UnitStats unitStats;

    public void Initialize(UnitStats stats)
    {
        unitStats = stats;
    }

    void Update()
    {
        if (unitStats != null)
        {
            healthSlider.value = unitStats.GetHealthNormalized();
        }
    }
}
