using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [Header("Estadísticas Base")]
    [SerializeField] private string entityName = "FideMon";
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("Ataques")]
    [SerializeField] private Attack[] attacks = new Attack[3];
    
    [Header("Referencias UI")]
    [SerializeField] private WorldHealthBar healthBar;
    
    [Header("Animación de Daño")]
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 3;
    [SerializeField] private Color damageColor = Color.red;
    
    [Header("Eventos")]
    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnDamageTaken;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;
    
    public string EntityName => entityName;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;
    public Attack[] Attacks => attacks;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void PerformAttack(int attackIndex, Entity target)
    {
        if (isDead || target == null || target.IsDead)
            return;
            
        if (attackIndex < 0 || attackIndex >= attacks.Length)
            return;
        
        Attack attack = attacks[attackIndex];
        if (attack == null)
            return;
        
        float damage = attack.BaseDamage;
        float damageVariation = Random.Range(0.85f, 1.15f);
        damage *= damageVariation;
        
        target.TakeDamage(damage);
        
        if (attack.AttackEffect != null)
        {
            Instantiate(attack.AttackEffect, target.transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
            
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        OnDamageTaken?.Invoke(damage);
        OnHealthChanged?.Invoke(currentHealth);
        
        StartCoroutine(DamageFlashEffect());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;
            
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        OnHealthChanged?.Invoke(currentHealth);
    }

    private IEnumerator DamageFlashEffect()
    {
        if (spriteRenderer == null)
            yield break;
            
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void Die()
    {
        if (isDead)
            return;
            
        isDead = true;
        OnDeath?.Invoke();
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = c;
            }
            
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.5f, t);
            yield return null;
        }
        
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        isDead = false;
        currentHealth = maxHealth;
        
        if (spriteRenderer != null)
        {
            Color c = originalColor;
            c.a = 1f;
            spriteRenderer.color = c;
        }
        
        transform.localScale = Vector3.one;
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        gameObject.SetActive(true);
    }
}