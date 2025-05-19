using UnityEngine;
using System;

public class CastController : MonoBehaviour
{
    public bool IsCasting { get; private set; }
    public float CastTime { get; private set; }
    public float ElapsedTime { get; private set; }
    public string SpellName { get; private set; }

    public event Action OnCastStart;
    public event Action OnCastEnd;
    public event Action OnCastInterrupt;

    private void Update()
    {
        if (!IsCasting) return;

        // Cancelar se o jogador se mover (simples verifica��o)
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.IsMoving)
        {
            InterruptCast();
            Debug.Log("Cast cancelado por movimento.");
            return;
        }

        ElapsedTime += Time.deltaTime;

        if (ElapsedTime >= CastTime)
        {
            EndCast();
        }
    }


    public void StartCast(string spellName, float duration)
    {
        if (CooldownManager.Instance != null && CooldownManager.Instance.IsGlobalCooldownActive)
        {
            Debug.Log("GCD ativo. Aguarde antes de lan�ar outra magia.");
            return;
        }

        IsCasting = true;
        CastTime = duration;
        ElapsedTime = 0f;
        SpellName = spellName;

        OnCastStart?.Invoke();
    }

    public void InterruptCast()
    {
        if (!IsCasting) return;

        IsCasting = false;
        OnCastInterrupt?.Invoke();
    }

    private void EndCast()
    {
        if (CooldownManager.Instance != null)
            CooldownManager.Instance.TriggerGlobalCooldown();


        IsCasting = false;
        OnCastEnd?.Invoke();
    }

    public float GetCastProgress()
    {
        return IsCasting ? Mathf.Clamp01(ElapsedTime / CastTime) : 0f;
    }

    public float GetRemainingTime()
    {
        return IsCasting ? Mathf.Max(CastTime - ElapsedTime, 0f) : 0f;
    }
}
