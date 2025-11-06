using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class BlueEnemyBehavior : MonoBehaviour
{
    [Header("Estados")]
    public bool isDmg;
    public bool isDead;

    [Header("Tags")]
    [SerializeField] private string damageTag = "Damage";
    [SerializeField] private string neutralTag = "Neutral";
    [SerializeField] private string invisibleTag = "invisible";

    [Header("Rotación")]
    [SerializeField] private float velocidadRotacion = 20f;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] public float attackRange = 2f;

    [Header("Movimiento")]
    private Quaternion rotacionInicial;
    private Rigidbody2D rb;
    public Animator animatorP;
    private GameObject player;

    [Header("Desfase de Animator (opcional)")]
    [SerializeField] private bool randomizeAnimatorStartTime = true;
    [SerializeField] private Vector2 normalizedTimeRange = new Vector2(0f, 1f); // 0..1 dentro del ciclo
    [SerializeField] private float animatorSpeedJitter = 0f; // p.ej. 0.1 -> ±10% de variación

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
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        desfaseAnimator();
    }

    private void desfaseAnimator()
    {
        // Desfase aleatorio del Animator para que no vayan sincronizados
        if (animatorP != null && randomizeAnimatorStartTime)
        {
            // Asegura que el Animator haya evaluado al estado por defecto (Idle, etc.)
            animatorP.Update(0f);

            // Cogemos el estado actual en la capa 0 (la habitual)
            var st = animatorP.GetCurrentAnimatorStateInfo(0);

            // Si es un estado en bucle (Idle suele serlo), saltamos a un punto aleatorio
            if (st.loop)
            {
                float t = Random.Range(normalizedTimeRange.x, normalizedTimeRange.y);
                animatorP.Play(st.fullPathHash, 0, t);
                animatorP.Update(0f); // aplica inmediatamente
            }

            // Variación opcional de velocidad para más “vida”
            if (animatorSpeedJitter > 0f)
            {
                float factor = 1f + Random.Range(-animatorSpeedJitter, animatorSpeedJitter);
                animatorP.speed *= factor;
            }
        }
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
        //si este objeto se acerca al jugador tanto como attackRange, Attack()
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if (!isDead && !isAttacking)
            {
                Attack();
            }
        }
    }



    private void Attack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animatorP.SetBool(IsAttackHash, true);
    }

    //Pinta gizmo que muestre attackRange en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
