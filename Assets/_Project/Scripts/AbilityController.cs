using UnityEngine;
using System;

public class AbilityController : MonoBehaviour
{
    public static AbilityController Instance;

    private GameObject currentCaster;
    private GameObject currentTarget;
    private AbilityBase currentAbility;


    [Header("Referências")]
    public GameObject player;
    public CastController castController;

   // [Header("VFX")]
    //public GameObject castingVFX;

    [Header("Fireball VFX")]
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public float fireballSpeed = 15f;



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        if (player == null)
            player = gameObject;
    }

    public void TryCastAbility(AbilityBase ability, GameObject caster, GameObject target)
    {
        if (ability == null || caster == null || target == null)
        {
            Debug.LogWarning("Ability, caster ou target estão nulos.");
            return;
        }

        if (CooldownManager.Instance != null &&
            CooldownManager.Instance.IsGlobalCooldownActive &&
            !ability.ignoreGlobalCooldown)
        {
            Debug.Log("Global Cooldown ativo. Aguarde...");
            return;
        }

        if (castController != null && castController.IsCasting)
        {
            Debug.Log("Já está conjurando uma magia. Aguarde o término.");
            return;
        }

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);
        if (distance > ability.range)
        {
            Debug.Log("Alvo fora do alcance.");
            return;
        }

        if (ability.castTime > 0f && castController != null)
        {
            currentAbility = ability;
            currentCaster = caster;
            currentTarget = target;

            castController.OnCastEnd -= OnCastEnd;
            castController.OnCastInterrupt -= OnCastInterrupt;

            castController.OnCastEnd += OnCastEnd;
            castController.OnCastInterrupt += OnCastInterrupt;

            castController.StartCast(ability.abilityName, ability.castTime);
            Debug.Log($"Iniciando cast de {ability.abilityName} por {ability.castTime} segundos.");

           // if (castingVFX != null)
          //      castingVFX.SetActive(true); // 👉 Ativa o VFX
        }
        else
        {
            ability.Execute(caster, target);
            Debug.Log($"{ability.abilityName} executada instantaneamente.");
            CooldownManager.Instance?.TriggerGlobalCooldown();
        }
    }

    private void OnCastEnd()
    {
        if (currentAbility == null || currentCaster == null || currentTarget == null)
        {
            Debug.LogWarning("Dados de conjuração inválidos.");
            CleanupCast();
            return;
        }

        float finalDistance = Vector3.Distance(currentCaster.transform.position, currentTarget.transform.position);
        if (finalDistance <= currentAbility.range)
        {
            currentAbility.Execute(currentCaster, currentTarget);
            Debug.Log($"{currentAbility.abilityName} executada após cast.");

            Debug.Log("Cast finalizado. Tentando lançar Fireball VFX...");

            if (fireballPrefab != null && fireballSpawnPoint != null)
            {
                GameObject fireballInstance = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

                FireballProjectile projectile = fireballInstance.GetComponent<FireballProjectile>();
                if (projectile != null)
                {
                    projectile.SetTarget(currentTarget.transform); // 🔥 ESSENCIAL
                    projectile.speed = fireballSpeed;              // se quiser ajustar no Inspector
                }

                fireballInstance.transform.forward = (currentTarget.transform.position - fireballSpawnPoint.position).normalized;
            }



            else
            {
                Debug.LogWarning("FireballVFX ou SpawnPoint está faltando no AbilityController.");
            }


        }
        else
        {
            Debug.Log("Alvo fora do alcance no final do cast.");
        }

        CooldownManager.Instance?.TriggerGlobalCooldown();
        CleanupCast();
    }

    private void OnCastInterrupt()
    {
        Debug.Log("Cast interrompido.");
        CleanupCast();
    }

    private void CleanupCast()
    {
        castController.OnCastEnd -= OnCastEnd;
        castController.OnCastInterrupt -= OnCastInterrupt;

        currentAbility = null;
        currentCaster = null;
        currentTarget = null;

      //  if (castingVFX != null)
        //    castingVFX.SetActive(false); // 👉 Desativa o VFX
    }

}
