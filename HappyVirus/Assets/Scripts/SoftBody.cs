
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class SoftBody : MonoBehaviour
{

    public GameObject blobsSkin;
    public GameObject blobsParent;
    public Vector3[] softPositions;
    
    public Vector3 skinPos;
    public Vector3 skinScale;
    
    private Quaternion softRot;
    public Quaternion skinRot;
    #region Constants;
    public float splineOffset = 0.5f;
    #endregion
    #region Fields;
    [SerializeField]
    public SpriteShapeController spriteShape;
    [SerializeField]
    public Transform [] points;
    #endregion

    #region MonoBehaviour Callbacks
void Awake()
{
    // Asegura el tamaño
    if (points == null || points.Length == 0)
    {
        Debug.LogError("SoftBody: 'points' vacío.");
        return;
    }

    softPositions = new Vector3[points.Length-1];
    UpdateVerticies(); // si necesitas esto en Awake
}

void Start()
{
    for (int i = 0; i < points.Length-1; i++)
        softPositions[i] = points[i].localPosition;

    softRot = points[0].rotation;
    skinPos = blobsSkin.transform.localPosition;
    skinRot = blobsSkin.transform.localRotation;
    skinScale = blobsSkin.transform.localScale;
}

    public void softBodyPosition()
    {
        for (int i = 0; i < points.Length-1; i++)
        {
            points[i].localPosition = softPositions[i];
            points[i].transform.rotation = softRot;
        }
        blobsSkin.gameObject.transform.localPosition = skinPos;
        blobsSkin.gameObject.transform.localRotation = skinRot;
        blobsSkin.gameObject.transform.localScale = skinScale;
        findSoftBodys();
    }
    
    public void findSoftBodys()
    {
        GameObject[] subViruses = GameObject.FindGameObjectsWithTag("SubVirus");
        foreach (GameObject subVirus in subViruses)
        {
            cloneMovement cloneMovement = subVirus.GetComponent<cloneMovement>();
            if (cloneMovement != null)
            {
                cloneMovement.softBodyPosition();
            }
        }
    }

    private void Update()
    {
        UpdateVerticies();
    }
    #endregion

    #region privateMethods
    private void UpdateVerticies()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 _vertex = points[i].localPosition;
            Vector2 _towardsCenter = (Vector2.zero - _vertex).normalized;
            float _colliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            try 
            { 
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * _colliderRadius));
            }
            catch 
            {
                Debug.Log("Los vertices estan muy cerca, recalculando");
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * (_colliderRadius + splineOffset)));
            }

            Vector2 _lt = spriteShape.spline.GetLeftTangent(i);

            Vector2 _newRt = Vector2.Perpendicular(_towardsCenter) * _lt.magnitude;
            Vector2 _newLt = Vector2.zero - (_newRt);

            spriteShape.spline.SetRightTangent(i, _newRt);
            spriteShape.spline.SetLeftTangent(i, _newLt);

        }

    }
    #endregion
}
