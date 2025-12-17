using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text healthText;
    [SerializeField] private bool smoothTransition = true;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private bool showHealthText = true;
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    [SerializeField] private float warningThreshold = 0.5f;
    [SerializeField] private float criticalThreshold = 0.25f;
    
    private float maxHealth;
    private float currentHealth;
    private float targetFillAmount;
    private Coroutine transitionCoroutine;

    private void Awake()
    {
        if (fillImage == null)
        {
            fillImage = transform.Find("Fill")?.GetComponent<Image>();
        }
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = health;
        
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
            UpdateHealthColor(1f);
        }
        
        UpdateHealthText();
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        targetFillAmount = currentHealth / maxHealth;
        
        if (smoothTransition)
        {
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }
            transitionCoroutine = StartCoroutine(SmoothHealthTransition());
        }
        else
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = targetFillAmount;
                UpdateHealthColor(targetFillAmount);
            }
        }
        
        UpdateHealthText();
    }

    private IEnumerator SmoothHealthTransition()
    {
        while (fillImage != null && !Mathf.Approximately(fillImage.fillAmount, targetFillAmount))
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * transitionSpeed);
            UpdateHealthColor(fillImage.fillAmount);
            
            if (Mathf.Abs(fillImage.fillAmount - targetFillAmount) < 0.001f)
            {
                fillImage.fillAmount = targetFillAmount;
                break;
            }
            
            yield return null;
        }
    }

    private void UpdateHealthColor(float fillAmount)
    {
        if (fillImage == null)
            return;
            
        if (fillAmount <= criticalThreshold)
        {
            fillImage.color = criticalColor;
        }
        else if (fillAmount <= warningThreshold)
        {
            fillImage.color = warningColor;
        }
        else
        {
            fillImage.color = healthyColor;
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null && showHealthText)
        {
            healthText.text = $"{Mathf.Ceil(currentHealth)}/{maxHealth}";
        }
    }

    public float GetHealthPercentage()
    {
        return maxHealth > 0 ? currentHealth / maxHealth : 0f;
    }
}
