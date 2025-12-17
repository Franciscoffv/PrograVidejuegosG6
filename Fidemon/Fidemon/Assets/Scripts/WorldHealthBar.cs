using UnityEngine;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private Transform targetEntity;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private float barWidth = 1f;
    [SerializeField] private float barHeight = 0.15f;
    [SerializeField] private Color backgroundColor = Color.gray;
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    [SerializeField] private float warningThreshold = 0.5f;
    [SerializeField] private float criticalThreshold = 0.25f;
    
    private SpriteRenderer backgroundRenderer;
    private SpriteRenderer fillRenderer;
    private float maxHealth = 100f;
    private float currentHealth = 100f;

    private void Awake()
    {
        CreateHealthBar();
    }

    private void LateUpdate()
    {
        if (targetEntity != null)
        {
            transform.position = targetEntity.position + offset;
        }
    }

    private void CreateHealthBar()
    {
        GameObject background = new GameObject("Background");
        background.transform.SetParent(transform);
        background.transform.localPosition = Vector3.zero;
        backgroundRenderer = background.AddComponent<SpriteRenderer>();
        backgroundRenderer.sprite = CreateSquareSprite();
        backgroundRenderer.color = backgroundColor;
        backgroundRenderer.sortingOrder = 99;
        background.transform.localScale = new Vector3(barWidth, barHeight, 1);
        
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(transform);
        fill.transform.localPosition = Vector3.zero;
        fillRenderer = fill.AddComponent<SpriteRenderer>();
        fillRenderer.sprite = CreateSquareSprite();
        fillRenderer.color = healthyColor;
        fillRenderer.sortingOrder = 100;
        fill.transform.localScale = new Vector3(barWidth, barHeight, 1);
    }

    private Sprite CreateSquareSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = health;
        UpdateBar();
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (fillRenderer == null)
            return;
            
        float healthPercent = maxHealth > 0 ? currentHealth / maxHealth : 0;
        
        Vector3 scale = fillRenderer.transform.localScale;
        scale.x = barWidth * healthPercent;
        fillRenderer.transform.localScale = scale;
        
        Vector3 pos = fillRenderer.transform.localPosition;
        pos.x = -(barWidth - scale.x) / 2f;
        fillRenderer.transform.localPosition = pos;
        
        if (healthPercent <= criticalThreshold)
        {
            fillRenderer.color = criticalColor;
        }
        else if (healthPercent <= warningThreshold)
        {
            fillRenderer.color = warningColor;
        }
        else
        {
            fillRenderer.color = healthyColor;
        }
    }

    public void SetTarget(Transform target)
    {
        targetEntity = target;
    }
}
