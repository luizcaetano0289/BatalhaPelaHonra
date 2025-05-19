using UnityEngine;

public class FireballLauncher : MonoBehaviour
{
    [Header("Refer�ncias")]
    public CastController castController;
    public DamageAbility fireballAbility;
    public GameObject target;

    [Header("Configura��o")]
    public float castTime = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Tecla 1 para lan�ar
        {
            if (!castController.IsCasting)
            {
                castController.StartCast("Fireball", castTime);
            }
        }
    }

    private void OnEnable()
    {
        castController.OnCastEnd += CastCompleted;
    }

    private void OnDisable()
    {
        castController.OnCastEnd -= CastCompleted;
    }

    private void CastCompleted()
    {
        if (fireballAbility != null && target != null)
        {
            fireballAbility.Execute(gameObject, target);
        }
        else
        {
            Debug.LogWarning("Fireball ou alvo n�o configurado.");
        }
    }
}
