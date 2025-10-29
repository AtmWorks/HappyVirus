using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cubequad.Tentacles2D
{
    public class TargetController : MonoBehaviour
    {
        [SerializeField] private float drag = .5f;
        private float angle, z;
        private new SpriteRenderer renderer;
        private Rigidbody2D rig;
        public float speed;
        private void Awake()
        {
            rig = this.GetComponent<Rigidbody2D>();
            renderer = GetComponent<SpriteRenderer>();
            z = transform.position.z;
            speed = 6;
        }

        private void Update()
        {

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);


            //para input normal
            rig.linearVelocity = movement * speed;
            if (Input.GetMouseButton(0))
            {
                var mousePosition = Input.mousePosition;
                mousePosition.z = 15f;
                var position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(mousePosition), .1f);
                transform.position = new Vector3(position.x, position.y, z);
            }
            //else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            //{
            //    if (renderer.isVisible)
            //    {
            //        transform.position += new Vector3(Input.GetAxis("Horizontal") * drag, Input.GetAxis("Vertical") * drag, 0);
            //    }
            //    else
            //    {
            //        transform.position = new Vector3(0, 0, z);
            //    }
            //}
        }
    }
}