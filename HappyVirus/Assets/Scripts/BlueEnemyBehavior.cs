using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class BlueEnemyBehavior : MonoBehaviour
{
    [Header("Estados")]
    public bool isDmg;
    public bool isDead;

    [Header("Tags")]
    [SerializeField] private string virusTag = "Virus";
    [SerializeField] private string damageTag = "Damage";
    [SerializeField] private string neutralTag = "Neutral";
    [SerializeField] private string invisibleTag = "invisible";

    [Header("Rotación")]
    [SerializeField] private float velocidadRotacion = 20f;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Movimiento")]
    private Quaternion rotacionInicial;
    private Rigidbody2D rb;
    public Animator animatorP;

    private static readonly int IsAttackHash = Animator.StringToHash("isAttack");

    private string currentTag;
    private bool isAttacking;
    private float attackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rotacionInicial = transform.rotation;   // restaurado: la rotación objetivo de partida
        currentTag = gameObject.tag;
        isDmg = false;
    }

    private void Update()
    {
        // Cooldown sin coroutines encadenadas
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
                animatorP.SetBool(IsAttackHash, false);
            }
        }

        // Actualiza el tag solo cuando cambie el estado
        string desiredTag =
            isDead ? invisibleTag :
            (isDmg ? damageTag : neutralTag);

        if (!ReferenceEquals(desiredTag, currentTag))
        {
            currentTag = desiredTag;
            gameObject.tag = currentTag;
        }
    }

    private void FixedUpdate()
    {
        // --- Rotación restaurada a tu lógica original (feel original) ---
        float anguloActual = transform.rotation.eulerAngles.z;
        if (anguloActual != rotacionInicial.eulerAngles.z)
        {
            float anguloObjetivo = rotacionInicial.eulerAngles.z;
            float rotacion = anguloObjetivo - anguloActual;
            if (rotacion > 180)
                rotacion -= 360;
            else if (rotacion < -180)
                rotacion += 360;

            rb.AddTorque(rotacion * velocidadRotacion); // sin normalizar ni clamp para mantener el “feel”
        }
        // ----------------------------------------------------------------
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Evita spam de ataques usando cooldown y estados
        if (!isDead && other.CompareTag(virusTag) && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animatorP.SetBool(IsAttackHash, true);
    }
}
