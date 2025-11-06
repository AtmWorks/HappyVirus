using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2DealerBehaviour : MonoBehaviour
{
    public GameObject spawner;
    public GameObject receiver;
    public GameObject O2move;
    public float speed = 5;

    public Rigidbody2D thisRB;

    public void Start()
    {
        instantiateO2();
    }
    public void instantiateO2()
    {
        var O2child = Instantiate(O2move, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        O2child.transform.parent = this.gameObject.transform;
    }
    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, receiver.transform.position, step);

    }
    
    //is there a way to ignore trigger enter from its own O2 child? Answer: yes, by checking if the collider's parent is this gameobject
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if the source of the collision is this gameObject, keep going, if its a child, ignore
        if (collision.transform.parent == this.transform) //explanation: if the parent of the collider is this gameobject, it means its a child //note: this is not working because ''collision'' is the object that collides, we want to know if THIS object that checks the collision is a child of the other object, to do that the only way
        {
            return;
        }
        if (collision.tag == "movingEnd")
        {
            this.transform.position = spawner.transform.position;
            //if there is no O2 child, instantiate one
            if (transform.childCount == 0)
            {
                instantiateO2();
            }
        }
    }
}
