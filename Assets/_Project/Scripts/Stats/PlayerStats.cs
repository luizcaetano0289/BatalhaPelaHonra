using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Classe do jogador")]
    public PlayerClass playerClass;

    [Header("Atributos Base")]
    public int baseStrength;
    public int baseAgility;
    public int baseIntellect;
    public int baseSpirit;
    public int baseStamina;
    public int baseArmor;

    [Header("Buffs (+)")]
    public int buffStrength;
    public int buffAgility;
    public int buffIntellect;
    public int buffSpirit;
    public int buffStamina;
    public int buffArmor;

    [Header("Debuffs (-)")]
    public int debuffStrength;
    public int debuffAgility;
    public int debuffIntellect;
    public int debuffSpirit;
    public int debuffStamina;
    public int debuffArmor;

    // Cálculo dos bônus totais
    public int bonusStrength => buffStrength + debuffStrength;
    public int bonusAgility => buffAgility + debuffAgility;
    public int bonusIntellect => buffIntellect + debuffIntellect;
    public int bonusSpirit => buffSpirit + debuffSpirit;
    public int bonusStamina => buffStamina + debuffStamina;
    public int bonusArmor => buffArmor + debuffArmor;

    // Totais
    public int TotalStrength => baseStrength + bonusStrength;
    public int TotalAgility => baseAgility + bonusAgility;
    public int TotalIntellect => baseIntellect + bonusIntellect;
    public int TotalSpirit => baseSpirit + bonusSpirit;
    public int TotalStamina => baseStamina + bonusStamina;
    public int TotalArmor => baseArmor + bonusArmor;

    // Derivados
    public float attackPower;
    public float spellPower;
    public float critChance;
    public float dodgeChance;
    public float manaRegen;
    public float blockValue;
    public float parryRating;
    public int maxHealth;
    public int maxMana;

    // Controle de tempo de buff/debuff
    private float buffTimerSTR = 0f;
    private float debuffTimerSTR = 0f;

    public int currentHealth;
    public int currentMana;


    private void Start()
    {
        RecalculateDerivedStats();

        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public void RecalculateDerivedStats()
    {
        maxHealth = 100 + TotalStamina * 10;
        maxMana = 100 + TotalIntellect * 15;

        switch (playerClass)
        {
            case PlayerClass.Guerreiro:
            case PlayerClass.Paladino:
            case PlayerClass.CavaleiroDaMorte:
                attackPower = TotalStrength * 2;
                critChance = TotalAgility / 83.33f;
                dodgeChance = TotalAgility / 62.5f;
                parryRating = TotalStrength * 0.27f;
                blockValue = TotalStrength * 0.5f;
                break;

            case PlayerClass.Cacador:
                attackPower = TotalStrength * 1 + TotalAgility * 2;
                critChance = TotalAgility / 83.33f;
                dodgeChance = TotalAgility / 62.5f;
                break;

            case PlayerClass.Ladino:
            case PlayerClass.Xama:
                attackPower = TotalStrength * 1 + TotalAgility * 1;
                critChance = TotalAgility / 83.33f;
                dodgeChance = TotalAgility / 62.5f;
                break;

            case PlayerClass.DruidaUrso:
                attackPower = TotalStrength * 2;
                critChance = TotalAgility / 83.33f;
                dodgeChance = TotalAgility / 62.5f;
                break;

            case PlayerClass.DruidaFelino:
                attackPower = TotalStrength * 2 + TotalAgility * 1;
                critChance = TotalAgility / 83.33f;
                dodgeChance = TotalAgility / 62.5f;
                break;

            case PlayerClass.Mago:
            case PlayerClass.Sacerdote:
            case PlayerClass.Bruxo:
                spellPower = TotalIntellect;
                critChance = TotalIntellect / 80f;
                manaRegen = TotalSpirit * 0.1f;
                break;
        }
    }
    private void Update()
    {
        if (buffTimerSTR > 0f)
        {
            buffTimerSTR -= Time.deltaTime;
            if (buffTimerSTR <= 0f)
            {
                buffStrength = 0;
                RecalculateDerivedStats();
            }
        }

        if (debuffTimerSTR > 0f)
        {
            debuffTimerSTR -= Time.deltaTime;
            if (debuffTimerSTR <= 0f)
            {
                debuffStrength = 0;
                RecalculateDerivedStats();
            }
        }
    }
    public void AplicarBuffTemporarioSTR(int valor, float duracao)
    {
        buffStrength = valor;
        buffTimerSTR = duracao;
        RecalculateDerivedStats();
    }

    public void AplicarDebuffTemporarioSTR(int valor, float duracao)
    {
        debuffStrength = valor;
        debuffTimerSTR = duracao;
        RecalculateDerivedStats();
    }


}