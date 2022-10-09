using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2DealerBehaviour : MonoBehaviour
{
    public GameObject spawner;
    public GameObject receiver;
    public GameObject O2move;
    public float speed;

    public Rigidbody2D thisRB;
    public void instantiateO2()
    {
        var O2child = Instantiate(O2move, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        O2child.transform.parent = this.gameObject.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = spawner.transform.position;
        speed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, receiver.transform.position, step);

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "movingEnd")
        {
            this.transform.position = spawner.transform.position;
            instantiateO2();
        }
    }
}
