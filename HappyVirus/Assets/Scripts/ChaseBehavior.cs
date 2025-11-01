using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Chaser2D : MonoBehaviour
{
    // =======================
    //        INSPECTOR
    // =======================

    [Header("🔗 Objetivo")]
    [Tooltip("Objeto a perseguir. Si está asignado manualmente, se usará este. Si queda vacío y 'objectTag' tiene valor, se buscará por Tag. Si no hay objetivo, no se persigue.")]
    public GameObject objectToChase;

    [Tooltip("Opcional. Si se rellena y 'objectToChase' está vacío, se buscará en escena el PRIMER objeto con este Tag y se asignará como objetivo.")]
    public string objectTag = "";

    [Header("⚙️ Movimiento")]
    [Tooltip("Velocidad máxima de persecución (unidades/s).")]
    public float maxSpeed = 5f;

    [Tooltip("Velocidad mínima utilizada por algunos modos (Incremental, Pulse, DistanceBased...).")]
    public float minimumSpeed = 1f;

    [Tooltip("Velocidad de rotación (grados/s) usada por modos incrementales/homing. Ignorada por 'Direct'.")]
    public float rotationSpeed = 360f;

    [Header("📏 Rangos de activación")]
    [Tooltip("Distancia a la que 'despierta' la persecución. Si el objetivo está a esta distancia o menos, se persigue.")]
    public float awakeRange = 7f;

    [Tooltip("Distancia a la que se deja de perseguir. Si el objetivo supera esta distancia, el enemigo abandona la persecución. Si es menor que 'awakeRange', se ajustará automáticamente a awakeRange + 2.")]
    public float stopRange = 10f;

    [Header("🧩 Control")]
    [Tooltip("Si está desactivado, se invalida la persecución desde este script (útil para control externo).")]
    public bool isAbleToChase = true;

    [Tooltip("Si es TRUE, cuando se deja de perseguir el enemigo volverá a su posición inicial. Si es FALSE, se queda donde está.")]
    public bool shouldBackToPoint = true;

    [Tooltip("Si es TRUE, la rotación es 'virtual': se calcula la dirección y el ángulo internamente, pero NO se gira físicamente el Rigidbody/Transform. Úsalo cuando otro script controle la rotación o para mantener el sprite sin voltearse.")]
    public bool isRotationVirtual = false;

    // =======================
    //     MODOS CONFIG
    // =======================

    public enum SpeedControl
    {
        Constant,        // Velocidad fija = maxSpeed
        Incremental,     // Lerp desde minimumSpeed -> maxSpeed
        Pulse,           // Bursts entre maxSpeed y minimumSpeed (tipo 'medusa')
        EaseInOut,       // Acelera suave y desacelera suave
        RandomBurst,     // Cambios aleatorios de velocidad en el rango
        MomentumDecay,   // Aumento inicial y decaimiento con el tiempo/impactos
        DistanceBased    // Menor velocidad cuanto más cerca del objetivo
    }

    public enum RotationControl
    {
        Direct,        // Apunta directamente al objetivo (ignora rotationSpeed)
        Incremental,   // Gira gradualmente con rotationSpeed
        Sinus,         // Oscila ±45° alrededor de la dirección al objetivo
        Predictive,    // Anticipa posición futura (usa Rigidbody2D del objetivo si existe)
        Jitter,        // Pequeñas variaciones aleatorias de ángulo
        Orbit,         // Órbita/espiral alrededor del objetivo mientras se acerca
        Homing         // Tipo misil teledirigido (ajuste continuo más agresivo)
    }

    [Header("🧮 Modos de velocidad y rotación")]
    [Tooltip("Define cómo evoluciona la velocidad.")]
    public SpeedControl speedMode = SpeedControl.Incremental;

    [Tooltip("Define cómo gira/encara el enemigo hacia el objetivo.")]
    public RotationControl rotationMode = RotationControl.Incremental;

    // =======================
    //     PARÁMETROS EXTRA
    // =======================

    [Header("🎚️ Tuning Velocidad (comunes)")]
    [Tooltip("Factor de interpolación por segundo para Incremental/EaseInOut.")]
    [Min(0f)] public float speedLerpRate = 2f;

    [Tooltip("Frecuencia de pulsos por segundo (Pulse).")]
    [Min(0f)] public float pulseFrequency = 1.2f;

    [Tooltip("Tiempo (s) medio entre cambios aleatorios (RandomBurst).")]
    [Min(0.05f)] public float randomBurstInterval = 0.75f;

    [Tooltip("Factor de decaimiento de momento por segundo (MomentumDecay). 0 = sin decaimiento, 1 = decae muy rápido.")]
    [Range(0f, 5f)] public float momentumDecayRate = 0.5f;

    [Tooltip("Multiplicador extra de aceleración al reanudar persecución tras frenar (MomentumDecay).")]
    [Range(0f, 5f)] public float momentumBoost = 1.0f;

    [Header("🧭 Tuning Rotación")]
    [Tooltip("Amplitud de oscilación (grados) para Sinus.")]
    [Range(0f, 90f)] public float sinusAmplitude = 45f;

    [Tooltip("Frecuencia de oscilación (Hz) para Sinus.")]
    [Min(0f)] public float sinusFrequency = 1.0f;

    [Tooltip("Tiempo de predicción (s) para apuntado Predictive.")]
    [Min(0f)] public float predictiveLeadTime = 0.35f;

    [Tooltip("Ángulo máximo de jitter (grados) para Jitter.")]
    [Range(0f, 45f)] public float jitterAngle = 6f;

    [Tooltip("Frecuencia de refresco del jitter (cambios por segundo).")]
    [Min(0.1f)] public float jitterFrequency = 6f;

    [Tooltip("Velocidad angular de la órbita (grados/s).")]
    public float orbitAngularSpeed = 180f;

    [Tooltip("Peso de la componente tangencial (0 = ir directo; 1 = órbita pura).")]
    [Range(0f, 1f)] public float orbitTangentWeight = 0.6f;

    [Tooltip("Ganancia de alineación para Homing (multiplica rotationSpeed).")]
    [Min(0f)] public float homingGain = 2.0f;

    [Header("↩️ Retorno a origen")]
    [Tooltip("Velocidad para volver al punto inicial cuando no se persigue (si shouldBackToPoint = true).")]
    public float returnSpeed = 3f;

    [Tooltip("Considerar que ya llegó al origen si está más cerca que este margen.")]
    public float returnArriveThreshold = 0.1f;

    // =======================
    //     ESTADO INTERNO
    // =======================

    private Rigidbody2D _rb;
    private Vector2 _startPosition;
    private bool _isChasing;
    private float _currentSpeed;
    private float _randomBurstTimer;
    private float _randomBurstTargetSpeed;
    private float _jitterTimer;
    private float _currentJitterAngle;

    // Cache
    private Rigidbody2D _targetRb;

    // Ángulo lógico que usamos cuando la rotación es virtual (o como fuente del ángulo actual en cálculos).
    private float _virtualAngle;

    // =======================
    //   CICLO DE VIDA
    // =======================

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = _rb.position;
        _virtualAngle = _rb.rotation; // inicializa ángulo virtual con el físico actual

        ValidateRanges();
        TryResolveTarget();
        CacheTargetRb();
        _currentSpeed = Mathf.Clamp(minimumSpeed, 0f, maxSpeed);
        ResetRandomBurst();
    }

    private void OnValidate()
    {
        // Mantener valores sanos en el editor
        if (maxSpeed < 0f) maxSpeed = 0f;
        if (minimumSpeed < 0f) minimumSpeed = 0f;
        if (minimumSpeed > maxSpeed) minimumSpeed = maxSpeed;

        if (awakeRange < 0f) awakeRange = 0f;
        if (stopRange < 0f) stopRange = 0f;

        ValidateRanges();
    }

    private void ValidateRanges()
    {
        // stopRange no puede ser menor que awakeRange -> forzar awakeRange + 2
        if (stopRange < awakeRange)
            stopRange = awakeRange + 2f;
    }

    private void CacheTargetRb()
    {
        _targetRb = null;
        if (objectToChase != null)
            _targetRb = objectToChase.GetComponent<Rigidbody2D>();
    }

    private void TryResolveTarget()
    {
        if (objectToChase == null && !string.IsNullOrEmpty(objectTag))
        {
            GameObject found = GameObject.FindWithTag(objectTag);
            if (found != null)
            {
                objectToChase = found;
                CacheTargetRb();
            }
        }
    }

    // =======================
    //     UPDATE FÍSICO
    // =======================

    private void FixedUpdate()
    {
        TryResolveTarget(); // Por si aparece en runtime
        if (!isAbleToChase || objectToChase == null)
        {
            _isChasing = false;
            HandleReturnToOrigin();
            return;
        }

        Vector2 myPos = _rb.position;
        Vector2 targetPos = GetAimedTargetPosition(Time.fixedDeltaTime);
        float distance = Vector2.Distance(myPos, targetPos);

        // Gestionar estados de persecución según distancias
        if (!_isChasing && distance <= awakeRange)
            _isChasing = true;
        else if (_isChasing && distance > stopRange)
            _isChasing = false;

        if (_isChasing)
        {
            Vector2 desiredDir = ComputeDesiredDirection(myPos, targetPos, Time.fixedDeltaTime);
            float speed = ComputeSpeed(distance, Time.fixedDeltaTime);
            MoveTowards(desiredDir, speed);
        }
        else
        {
            HandleReturnToOrigin();
        }
    }

    // =======================
    //      MOVIMIENTO
    // =======================

    private void MoveTowards(Vector2 desiredDir, float speed)
    {
        if (desiredDir.sqrMagnitude < 1e-6f) return;
        desiredDir.Normalize();

        // Rotación física o virtual
        ApplyRotation(desiredDir, Time.fixedDeltaTime);

        // Avance usando MovePosition (respeta colisiones mejor que Translate)
        Vector2 delta = desiredDir * speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + delta);
    }

    private void HandleReturnToOrigin()
    {
        if (!shouldBackToPoint) return;

        Vector2 toHome = _startPosition - _rb.position;
        float distHome = toHome.magnitude;

        if (distHome <= returnArriveThreshold) return;

        Vector2 dir = toHome / Mathf.Max(distHome, 1e-6f);
        ApplyRotation(dir, Time.fixedDeltaTime); // actualiza física o virtual según la configuración
        Vector2 delta = dir * Mathf.Min(returnSpeed, maxSpeed) * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + delta);
    }

    // =======================
    //  DIRECCIÓN/ROTACIÓN
    // =======================

    private Vector2 ComputeDesiredDirection(Vector2 myPos, Vector2 targetPos, float dt)
    {
        Vector2 toTarget = targetPos - myPos;
        if (toTarget.sqrMagnitude < 1e-6f) return Vector2.zero;

        Vector2 baseDir = toTarget.normalized;

        switch (rotationMode)
        {
            case RotationControl.Direct:
                return baseDir;

            case RotationControl.Incremental:
                return RotateIncremental(baseDir, dt);

            case RotationControl.Sinus:
                return ApplySinus(baseDir, dt);

            case RotationControl.Predictive:
            {
                Vector2 predictiveDir = (GetPredictivePoint(myPos, dt) - myPos).normalized;
                return RotateTowardsCurrent(predictiveDir, dt);
            }

            case RotationControl.Jitter:
                return ApplyJitter(baseDir, dt);

            case RotationControl.Orbit:
                return ApplyOrbit(baseDir, dt);

            case RotationControl.Homing:
            {
                Vector2 homingDir = baseDir; // base hacia el objetivo
                return RotateHoming(homingDir, dt);
            }
        }

        return baseDir;
    }

    private void ApplyRotation(Vector2 desiredDir, float dt)
    {
        if (desiredDir.sqrMagnitude < 1e-6f) return;

        float desiredAngle = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg;

        if (rotationMode == RotationControl.Direct)
        {
            // En Direct, alineamos instantáneamente
            SetCurrentAngle(desiredAngle);
            return;
        }

        float currentAngle = GetCurrentAngle();
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, rotationSpeed * dt);
        SetCurrentAngle(newAngle);
    }

    // Lee el ángulo actual desde el cuerpo físico o desde el ángulo virtual
    private float GetCurrentAngle()
    {
        return isRotationVirtual ? _virtualAngle : _rb.rotation;
    }

    // Escribe el ángulo actual en el cuerpo físico o solo en el ángulo virtual
    private void SetCurrentAngle(float angle)
    {
        if (isRotationVirtual)
        {
            _virtualAngle = angle; // no giramos físicamente
        }
        else
        {
            _rb.MoveRotation(angle); // rotación real
        }
    }

    private Vector2 RotateIncremental(Vector2 desiredDir, float dt)
    {
        float desiredAngle = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg;
        float currentAngle = GetCurrentAngle();
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, rotationSpeed * dt);
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    private Vector2 ApplySinus(Vector2 baseDir, float dt)
    {
        float t = Time.time;
        float offset = Mathf.Sin(t * Mathf.PI * 2f * Mathf.Max(0.0001f, sinusFrequency)) * sinusAmplitude;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;
        float currentAngle = GetCurrentAngle();
        float targetAngle = baseAngle + offset;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * dt);
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    private Vector2 ApplyJitter(Vector2 baseDir, float dt)
    {
        _jitterTimer -= dt;
        if (_jitterTimer <= 0f)
        {
            _jitterTimer = 1f / Mathf.Max(0.0001f, jitterFrequency);
            _currentJitterAngle = Random.Range(-jitterAngle, jitterAngle);
        }

        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;
        float desiredAngle = baseAngle + _currentJitterAngle;
        float newAngle = Mathf.MoveTowardsAngle(GetCurrentAngle(), desiredAngle, rotationSpeed * dt);
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    private Vector2 ApplyOrbit(Vector2 baseDir, float dt)
    {
        // Tangente a la derecha de la dirección al objetivo
        Vector2 tangent = new Vector2(-baseDir.y, baseDir.x);
        // Mezcla entre ir directo y tangencial (controla el espiral vs órbita)
        Vector2 mixed = Vector2.Lerp(baseDir, tangent, orbitTangentWeight).normalized;

        // Aproximamos el ángulo objetivo hacia 'mixed'
        float currentAngle = GetCurrentAngle();
        float targetAngle = Mathf.Atan2(mixed.y, mixed.x) * Mathf.Rad2Deg;
        float angularStep = orbitAngularSpeed * dt;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, angularStep);
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    private Vector2 RotateHoming(Vector2 desiredDir, float dt)
    {
        float desiredAngle = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg;
        float currentAngle = GetCurrentAngle();
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, rotationSpeed * homingGain * dt);
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    private Vector2 RotateTowardsCurrent(Vector2 targetDir, float dt)
    {
        float desiredAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        float newAngle = Mathf.MoveTowardsAngle(GetCurrentAngle(), desiredAngle, rotationSpeed * dt);
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    private Vector2 GetPredictivePoint(Vector2 myPos, float dt)
    {
        // Si el objetivo tiene Rigidbody2D, anticipa su posición futura
        if (_targetRb != null)
        {
            Vector2 targetVel = _targetRb.linearVelocity;
            Vector2 lead = targetVel * predictiveLeadTime;
            return (Vector2)_targetRb.position + lead;
        }
        return (Vector2)objectToChase.transform.position;
    }

    private Vector2 GetAimedTargetPosition(float dt)
    {
        switch (rotationMode)
        {
            case RotationControl.Predictive:
                return GetPredictivePoint(_rb.position, dt);
            default:
                return objectToChase != null ? (Vector2)objectToChase.transform.position : _rb.position;
        }
    }

    // =======================
    //     CONTROL VELOCIDAD
    // =======================

    private float ComputeSpeed(float distance, float dt)
    {
        float speed = maxSpeed;

        switch (speedMode)
        {
            case SpeedControl.Constant:
                speed = maxSpeed;
                break;

            case SpeedControl.Incremental:
                // Sube de minimum -> max con Lerp exponencial controlado por speedLerpRate
                _currentSpeed = Mathf.Lerp(_currentSpeed, maxSpeed, 1f - Mathf.Exp(-speedLerpRate * dt));
                _currentSpeed = Mathf.Clamp(_currentSpeed, minimumSpeed, maxSpeed);
                speed = _currentSpeed;
                break;

            case SpeedControl.Pulse:
            {
                // Oscila con PingPong entre min y max
                float t = Mathf.PingPong(Time.time * Mathf.Max(0.0001f, pulseFrequency), 1f);
                speed = Mathf.Lerp(minimumSpeed, maxSpeed, t);
                _currentSpeed = speed;
                break;
            }

            case SpeedControl.EaseInOut:
            {
                // Acelera suave y desacelera suave usando SmoothStep con una 'meta' dinámica
                float t = 1f - Mathf.Exp(-speedLerpRate * dt);
                _currentSpeed = Mathf.Lerp(_currentSpeed, maxSpeed, t);
                float smooth = Mathf.SmoothStep(minimumSpeed, maxSpeed, Mathf.InverseLerp(minimumSpeed, maxSpeed, _currentSpeed));
                speed = Mathf.Clamp(smooth, minimumSpeed, maxSpeed);
                break;
            }

            case SpeedControl.RandomBurst:
            {
                _randomBurstTimer -= dt;
                if (_randomBurstTimer <= 0f)
                    ResetRandomBurst();

                // Lerp hacia un objetivo de velocidad aleatorio
                _currentSpeed = Mathf.Lerp(_currentSpeed, _randomBurstTargetSpeed, 1f - Mathf.Exp(-speedLerpRate * dt));
                _currentSpeed = Mathf.Clamp(_currentSpeed, minimumSpeed, maxSpeed);
                speed = _currentSpeed;
                break;
            }

            case SpeedControl.MomentumDecay:
            {
                // Si estamos en persecución recientemente, tender a subir; si no, decaer
                float grow = (speedLerpRate + momentumBoost) * dt;
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, maxSpeed, grow);
                // Decaimiento continuo
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, minimumSpeed, momentumDecayRate * dt);
                _currentSpeed = Mathf.Clamp(_currentSpeed, minimumSpeed, maxSpeed);
                speed = _currentSpeed;
                break;
            }

            case SpeedControl.DistanceBased:
            {
                // Menor velocidad cuanto más cerca del objetivo: mapear distancia 0..awakeRange -> min..max
                float t = Mathf.InverseLerp(0f, Mathf.Max(awakeRange, 0.0001f), distance);
                speed = Mathf.Lerp(minimumSpeed, maxSpeed, t);
                _currentSpeed = speed;
                break;
            }
        }

        return speed;
    }

    private void ResetRandomBurst()
    {
        _randomBurstTimer = Random.Range(0.5f * randomBurstInterval, 1.5f * randomBurstInterval);
        _randomBurstTargetSpeed = Random.Range(minimumSpeed, Mathf.Max(minimumSpeed, maxSpeed));
    }

    // =======================
    //   DEPURACIÓN OPCIONAL
    // =======================

    private void OnDrawGizmosSelected()
    {
        // Rangos awake/stop
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, awakeRange);

        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, stopRange);

        // Origen
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(0f, 0.5f, 1f, 0.35f);
            Gizmos.DrawSphere(_startPosition, 0.08f);
        }
    }
}
