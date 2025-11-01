// using UnityEngine;

// public class SoftBodyPosition : MonoBehaviour
// {
//     public GameObject VirusSkin;
//     public GameObject VirusBody;

//     public GameObject blob1;
//     public GameObject blob2;
//     public GameObject blob3;
//     public GameObject blob4;
//     public GameObject blob5;
//     public GameObject blob6;

//     private Vector3 softPos1;
//     private Vector3 softPos2;
//     private Vector3 softPos3;
//     private Vector3 softPos4;
//     private Vector3 softPos5;
//     private Vector3 softPos6;
    
//     public Vector3 skinPos;
//     public Vector3 skinScale;
    
//     private Quaternion softRot;
//     public Quaternion skinRot;

//     void Start()
//     {
//         // Guardamos posiciones del soft 
//         softPos1 = blob1.transform.localPosition;
//         softPos2 = blob2.transform.localPosition;
//         softPos3 = blob3.transform.localPosition;
//         softPos4 = blob4.transform.localPosition;
//         softPos5 = blob5.transform.localPosition;
//         softPos6 = blob6.transform.localPosition;
//         softRot = blob1.transform.rotation;
//         skinPos = VirusSkin.transform.localPosition;
//         skinRot = VirusSkin.transform.localRotation;
//         skinScale = VirusSkin.transform.localScale;
//     }

//     public void softBodyPosition()
//     {
//         VirusBody.SetActive(true);
//         blob1.transform.localPosition = softPos1;
//         blob2.transform.localPosition = softPos2;
//         blob3.transform.localPosition = softPos3;
//         blob4.transform.localPosition = softPos4;
//         blob5.transform.localPosition = softPos5;
//         blob6.transform.localPosition = softPos6;
//         blob1.transform.rotation = softRot;
//         blob2.transform.rotation = softRot;
//         blob3.transform.rotation = softRot;
//         blob4.transform.rotation = softRot;
//         blob5.transform.rotation = softRot;
//         blob6.transform.rotation = softRot;
//         VirusSkin.gameObject.transform.localPosition = skinPos;
//         VirusSkin.gameObject.transform.localRotation = skinRot;
//         VirusSkin.gameObject.transform.localScale = skinScale;
//         findSoftBodys();
//     }

//     public void findSoftBodys()
//     {
//         GameObject[] subViruses = GameObject.FindGameObjectsWithTag("SubVirus");
//         foreach (GameObject subVirus in subViruses)
//         {
//             cloneMovement cloneMovement = subVirus.GetComponent<cloneMovement>();
//             if (cloneMovement != null)
//             {
//                 cloneMovement.softBodyPosition();
//             }
//         }
//     }

// }
