using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float travelTime = 5f;

    private new Rigidbody2D rigidbody2D;

    private Vector3 startPosition;

    private Vector3 velocity;

    private List<Rigidbody2D> passengers;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        startPosition = transform.position;

        velocity = (endPoint.position - startPosition) / travelTime;

        rigidbody2D.linearVelocity = velocity;

        passengers = new List<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        endPoint.position -= velocity * Time.fixedDeltaTime;
        if ((startPosition - transform.position).magnitude > (endPoint.position - startPosition).magnitude)
        {
            velocity = -(endPoint.position - startPosition) / travelTime;

            rigidbody2D.linearVelocity = velocity;
        }
        else if ((endPoint.position - transform.position).magnitude > (endPoint.position - startPosition).magnitude)
        {
            velocity = (endPoint.position - startPosition) / travelTime;

            rigidbody2D.linearVelocity = velocity;
        }

        foreach (var rigidbody in passengers)
        {
            if (rigidbody != null)
            {
                rigidbody.linearVelocity += (Vector2) velocity;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigidbody = collision.rigidbody;
        if (rigidbody != null && rigidbody != rigidbody2D && !passengers.Contains(rigidbody))
        {
            passengers.Add(rigidbody);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rigidbody = collision.rigidbody;
        if (rigidbody != null && passengers.Contains(rigidbody))
        {
            passengers.Remove(rigidbody);
        }
    }
}
