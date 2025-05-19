using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    [Header("Geral")]
    public string abilityName = "Nova Magia";
    public Sprite icon;
    public float castTime = 1.5f;
    public float cooldown = 3f;
    public float range = 20f;
    public bool ignoreGlobalCooldown = false;

    public abstract void Execute(GameObject caster, GameObject target);
}
