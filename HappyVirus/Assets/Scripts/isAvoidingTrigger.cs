using System.Collections.Generic;
using UnityEngine;

public class TentacleProximityChecker : MonoBehaviour
{
    [Header("Referencia al script de la medusa")]
    public JellyfishMovement jellyfish;

    [Header("Distancia mínima para evitar tentáculo")]
    public float avoidDistance = 1f;

    public List<Transform> tentacleTips = new List<Transform>();

    private void Start()
    {
        GameObject[] tips = GameObject.FindGameObjectsWithTag("TentacleTip");
        foreach (GameObject tip in tips)
        {
            tentacleTips.Add(tip.transform);
        }
    }

    private void Update()
    {
        bool anyClose = false;

        float sqrAvoidDistance = avoidDistance * avoidDistance;

        foreach (Transform tip in tentacleTips)
        {
            if (tip == null) continue;

            float sqrDist = (tip.position - transform.position).sqrMagnitude;
            if (sqrDist < sqrAvoidDistance)
            {
                anyClose = true;
                break;
            }
        }

        jellyfish.isAvoiding = anyClose;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TentacleTip"))
            jellyfish.isAvoiding = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("TentacleTip"))
            jellyfish.isAvoiding = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TentacleTip"))
            jellyfish.isAvoiding = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, avoidDistance);
    }
}