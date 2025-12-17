using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [Header("FideMon en Combate")]
    [SerializeField] private Entity playerEntity;
    [SerializeField] private Entity enemyEntity;
    [SerializeField] private GameObject attackButtonsPanel;
    [SerializeField] private Button[] attackButtons = new Button[3];
    [SerializeField] private Text[] attackButtonTexts = new Text[3];
    [SerializeField] private Text battleLogText;
    [SerializeField] private Text turnIndicatorText;
    [SerializeField] private float timeBetweenActions = 1.5f;
    [SerializeField] private bool playerGoesFirst = true;
    [SerializeField] private GameObject battleEndPanel;
    [SerializeField] private Text battleResultText;
    
    private bool isPlayerTurn;
    private bool battleEnded = false;
    private bool isProcessingTurn = false;

    private void Start()
    {
        InitializeBattle();
    }

    public void InitializeBattle()
    {
        battleEnded = false;
        isProcessingTurn = false;
        isPlayerTurn = playerGoesFirst;
        
        SetupAttackButtons();
        
        if (playerEntity != null)
        {
            playerEntity.OnDeath.AddListener(OnPlayerDefeated);
        }
        
        if (enemyEntity != null)
        {
            enemyEntity.OnDeath.AddListener(OnEnemyDefeated);
        }
        
        if (battleEndPanel != null)
        {
            battleEndPanel.SetActive(false);
        }
        
        UpdateTurnIndicator();
        UpdateBattleLog("¡La batalla comienza!");
        
        if (!isPlayerTurn)
        {
            StartCoroutine(ProcessEnemyTurn());
        }
    }

    private void SetupAttackButtons()
    {
        if (playerEntity == null || playerEntity.Attacks == null)
            return;
            
        for (int i = 0; i < attackButtons.Length; i++)
        {
            if (attackButtons[i] == null)
                continue;
                
            int attackIndex = i;
            attackButtons[i].onClick.RemoveAllListeners();
            
            if (i < playerEntity.Attacks.Length && playerEntity.Attacks[i] != null)
            {
                Attack attack = playerEntity.Attacks[i];
                
                if (attackButtonTexts[i] != null)
                {
                    attackButtonTexts[i].text = $"{attack.AttackName}\n({attack.BaseDamage} DMG)";
                }
                
                attackButtons[i].onClick.AddListener(() => OnAttackButtonPressed(attackIndex));
                attackButtons[i].gameObject.SetActive(true);
            }
            else
            {
                attackButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnAttackButtonPressed(int attackIndex)
    {
        if (!isPlayerTurn || battleEnded || isProcessingTurn)
            return;
            
        StartCoroutine(ProcessPlayerAttack(attackIndex));
    }

    private IEnumerator ProcessPlayerAttack(int attackIndex)
    {
        isProcessingTurn = true;
        SetAttackButtonsInteractable(false);
        
        Attack attack = playerEntity.Attacks[attackIndex];
        UpdateBattleLog($"{playerEntity.EntityName} usa {attack.AttackName}!");
        
        playerEntity.PerformAttack(attackIndex, enemyEntity);
        
        yield return new WaitForSeconds(timeBetweenActions);
        
        if (!battleEnded)
        {
            isPlayerTurn = false;
            UpdateTurnIndicator();
            StartCoroutine(ProcessEnemyTurn());
        }
        
        isProcessingTurn = false;
    }

    private IEnumerator ProcessEnemyTurn()
    {
        isProcessingTurn = true;
        SetAttackButtonsInteractable(false);
        
        yield return new WaitForSeconds(0.5f);
        
        if (battleEnded || enemyEntity == null || enemyEntity.IsDead)
        {
            isProcessingTurn = false;
            yield break;
        }
        
        int availableAttacks = 0;
        for (int i = 0; i < enemyEntity.Attacks.Length; i++)
        {
            if (enemyEntity.Attacks[i] != null)
                availableAttacks++;
        }
        
        if (availableAttacks == 0)
        {
            isProcessingTurn = false;
            yield break;
        }
        
        int selectedAttack;
        do
        {
            selectedAttack = Random.Range(0, enemyEntity.Attacks.Length);
        } while (enemyEntity.Attacks[selectedAttack] == null);
        
        Attack attack = enemyEntity.Attacks[selectedAttack];
        UpdateBattleLog($"{enemyEntity.EntityName} usa {attack.AttackName}!");
        
        enemyEntity.PerformAttack(selectedAttack, playerEntity);
        
        yield return new WaitForSeconds(timeBetweenActions);
        
        if (!battleEnded)
        {
            isPlayerTurn = true;
            UpdateTurnIndicator();
            SetAttackButtonsInteractable(true);
        }
        
        isProcessingTurn = false;
    }

    private void SetAttackButtonsInteractable(bool interactable)
    {
        foreach (Button button in attackButtons)
        {
            if (button != null)
            {
                button.interactable = interactable;
            }
        }
    }

    private void UpdateTurnIndicator()
    {
        if (turnIndicatorText != null)
        {
            turnIndicatorText.text = isPlayerTurn ? "Tu turno" : "Turno enemigo";
        }
    }

    private void UpdateBattleLog(string message)
    {
        if (battleLogText != null)
        {
            battleLogText.text = message;
        }
    }

    private void OnPlayerDefeated()
    {
        EndBattle(false);
    }

    private void OnEnemyDefeated()
    {
        EndBattle(true);
    }

    private void EndBattle(bool playerWon)
    {
        battleEnded = true;
        SetAttackButtonsInteractable(false);
        
        string resultMessage = playerWon 
            ? $"¡Victoria! {enemyEntity.EntityName} ha sido derrotado!" 
            : $"Derrota... {playerEntity.EntityName} ha sido derrotado.";
            
        UpdateBattleLog(resultMessage);
        
        if (battleEndPanel != null)
        {
            battleEndPanel.SetActive(true);
            
            if (battleResultText != null)
            {
                battleResultText.text = resultMessage;
            }
        }
    }

    public void RestartBattle()
    {
        if (playerEntity != null)
        {
            playerEntity.Reset();
        }
        
        if (enemyEntity != null)
        {
            enemyEntity.Reset();
        }
        
        InitializeBattle();
    }
}