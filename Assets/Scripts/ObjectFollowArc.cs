using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowArc : MonoBehaviour
{
    public Transform startPoint; // The starting point of the projectile arc
    public Transform endPoint;   // The ending point of the projectile arc
    public float speed = 2.0f;   // Speed of object movement

    private bool isFollowing = false;

    void Update()
    {
        if (isFollowing)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, step);
        }
    }

    public void StartFollowing()
    {
        isFollowing = true;
        transform.position = startPoint.position;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }
}
