using UnityEngine;

[RequireComponent(typeof(SimpleHealth))]
public class Enemy : MonoBehaviour
{
    public int contactDamage = 10;
    public bool hurtOnContact = true;

    private SimpleHealth health;

    void Awake()
    {
        health = GetComponent<SimpleHealth>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!hurtOnContact) return;
        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent<SimpleHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(contactDamage);
        }
    }
}
