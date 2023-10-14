using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBool : MonoBehaviour
{
    public class PlayerAnimator : MonoBehaviour
    {
        // Start is called before the first frame update
        public bool isOnTrigger;
        public GameObject enemy;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == ("Enemy"))
            {
                isOnTrigger = true;
                enemy = collision.gameObject;
            }
        }
    }
}
