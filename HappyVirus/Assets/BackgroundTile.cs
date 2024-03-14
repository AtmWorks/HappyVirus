using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject player;
    public float desiredDistance;

    void Update()
    {
        // Calcular la distancia en X y Y entre el jugador y este objeto
        float distanceX = transform.position.x - player.transform.position.x;
        float distanceY = transform.position.y - player.transform.position.y;

        // Si la distancia en X es mayor que la distancia deseada, teletransportar al lado opuesto
        if (Mathf.Abs(distanceX) > desiredDistance)
        {
            // Calcular la nueva posición en X teniendo en cuenta el lado opuesto al jugador
            float newX = player.transform.position.x + Mathf.Sign(distanceX) * (desiredDistance * -1);

            // Teletransportar el objeto a la nueva posición
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        // Si la distancia en Y es mayor que la distancia deseada, teletransportar al lado opuesto
        if (Mathf.Abs(distanceY) > desiredDistance)
        {
            // Calcular la nueva posición en Y teniendo en cuenta el lado opuesto al jugador
            float newY = player.transform.position.y + Mathf.Sign(distanceY) * (desiredDistance * -1);

            // Teletransportar el objeto a la nueva posición
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
