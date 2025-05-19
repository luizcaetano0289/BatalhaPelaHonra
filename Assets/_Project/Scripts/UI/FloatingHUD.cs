using UnityEngine;
using UnityEngine.UI;

public class FloatingHUD : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider castSlider;

    [Header("Velocidade da animação")]
    [SerializeField] private float castFillSpeed = 5f;

    private UnitStats unitStats;
    private float visualCastValue = 0f;

    public void Initialize(UnitStats stats)
    {
        unitStats = stats;

        healthSlider.value = stats.GetHealthNormalized();
        castSlider.gameObject.SetActive(false);
        visualCastValue = 0f;
    }

    private void Update()
    {
        if (unitStats != null)
        {
            // Atualiza barra de vida
            healthSlider.value = unitStats.GetHealthNormalized();

            // Atualiza barra de cast
            if (unitStats.isCasting)
            {
                castSlider.gameObject.SetActive(true);
                float targetValue = unitStats.GetCastProgress();
                visualCastValue = Mathf.MoveTowards(visualCastValue, targetValue, Time.deltaTime * castFillSpeed);
                castSlider.value = visualCastValue;
            }
            else
            {
                castSlider.gameObject.SetActive(false);
                visualCastValue = 0f;
            }

            // Rotaciona para câmera
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
