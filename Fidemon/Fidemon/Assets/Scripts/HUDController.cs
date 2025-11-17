using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public SimpleHealth playerHealth;
    public Slider hpSlider;
    public Text hpText;
    public Text scoreText;
    private int score = 0;

    void Start()
    {
        if (playerHealth != null)
        {
            hpSlider.maxValue = playerHealth.maxHp;
            hpSlider.value = playerHealth.currentHp;
            playerHealth.onTakeDamage.AddListener(OnPlayerDamaged);
        }
        UpdateScoreText();
    }

    void OnPlayerDamaged(int dmg)
    {
        hpSlider.value = playerHealth.currentHp;
        if (hpText) hpText.text = $"{playerHealth.currentHp}/{playerHealth.maxHp}";
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText) scoreText.text = $"Score: {score}";
    }
}
