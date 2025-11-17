using UnityEngine;
using UnityEngine.Events;

public class SimpleHealth : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;
    public UnityEvent onDeath;
    public UnityEvent<int> onTakeDamage; // pasa el daño recibido

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        onTakeDamage?.Invoke(amount);
        if (currentHp <= 0)
        {
            currentHp = 0;
            onDeath?.Invoke();
            // Destroy(gameObject); // o animación de muerte
        }
    }

    public void Heal(int amount)
    {
        currentHp = Mathf.Min(maxHp, currentHp + amount);
    }
}
