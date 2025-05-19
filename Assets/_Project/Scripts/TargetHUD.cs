using UnityEngine;
using UnityEngine.UI;

public class TargetHUD : MonoBehaviour
{
    public Slider healthSlider;
    private UnitStats targetStats;

    public void SetTarget(UnitStats stats)
    {
        targetStats = stats;
    }

    void Update()
    {
        if (targetStats != null)
        {
            healthSlider.value = targetStats.GetHealthNormalized();
        }
    }
}
