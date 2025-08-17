using UnityEngine;

public class BlueWallShooter : MonoBehaviour
{
    [Header("References")]
    public GameObject proyectil;
    public Transform shootPoint;
    public Animator thisAnimator;
    public GameObject objetToRotate;

    [Header("Config")]
    public float rotationSpeed = 5f;
    public float timeBetweenAttacks = 1f;

    private GameObject virus = null;
    private float initialRotation;
    private float timer;
    public bool attack;
    void Start()
    {
        if (objetToRotate != null)
            initialRotation = objetToRotate.transform.rotation.eulerAngles.z;

        timer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Virus") && virus == null)
        {
            virus = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus") && collision.gameObject == virus)
        {
            virus = null;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (virus != null)
        {
            RotateTowards(virus.transform.position);

            float angleDifference = Quaternion.Angle(
                objetToRotate.transform.rotation,
                Quaternion.LookRotation(Vector3.forward, virus.transform.position - objetToRotate.transform.position)
            );

            if (angleDifference < 1f && timer <= 0f)
            {
                thisAnimator.SetBool("canAttack", true);
            }
        }
        else
        {
            RotateTowards(new Vector3(0, 0, initialRotation), isAngle:true);
        }
        if (attack && timer < 0)
        {
            attack = false;
            Instantiate(proyectil, shootPoint.position, shootPoint.rotation);
            timer = timeBetweenAttacks;
            attack = false;
            thisAnimator.SetBool("canAttack", false);
        }
    }

    // Called from Animation Event


    private void RotateTowards(Vector3 target, bool isAngle = false)
    {
        Quaternion targetRotation = isAngle 
            ? Quaternion.Euler(0, 0, target.z) 
            : Quaternion.LookRotation(Vector3.forward, target - objetToRotate.transform.position);

        objetToRotate.transform.rotation = Quaternion.Slerp(
            objetToRotate.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
