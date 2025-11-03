using UnityEngine;

public class meatSacWallEnemyHiddingTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;           // Debe asignarse en el inspector
    [SerializeField] private string isHiddingParam = "isHidding";
    public float hideCooldown = 3f;

    // --------------------------------------------------------------------
    // Detección de amenazas dentro del Trigger
    // Usa Invoke para optimizar el cooldown sin Update
    // --------------------------------------------------------------------
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!animator) return;

        if (other.CompareTag("Proyectil") || other.CompareTag("Virus"))
        {
            // Entra en hiding mientras haya amenazas
            if (!animator.GetBool(isHiddingParam))
                animator.SetBool(isHiddingParam, true);

            // Reinicia el temporizador de "salir de hiding"
            CancelInvoke(nameof(StopHiding));
            Invoke(nameof(StopHiding), hideCooldown);
        }
    }

    // Se llama 3s después del último OnTriggerStay2D
    private void StopHiding()
    {
        if (!animator) return;
        animator.SetBool(isHiddingParam, false);
    }
}
