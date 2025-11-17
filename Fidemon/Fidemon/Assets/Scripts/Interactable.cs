using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public float interactDistance = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    public UnityEvent onInteract; // asigna en el Inspector

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;
        if (Vector2.Distance(transform.position, player.position) <= interactDistance && Input.GetKeyDown(interactKey))
        {
            onInteract?.Invoke();
        }
    }
}

