using UnityEngine;
using System.Collections;

public class CollisionListener : MonoBehaviour
{
    public delegate void OnCollision(Collision collision);

    public delegate void OnCollider(Collider collider);

    public OnCollision CollisionEnter;

    public OnCollision CollisionStay;

    public OnCollision CollisionExit;

    public OnCollider TriggerEnter;

    public OnCollider TriggerStay;

    public OnCollider TriggerExit;

    public static CollisionListener Get(Rigidbody body)
    {
        CollisionListener listener = body.GetComponent<CollisionListener>();
        if (listener == null)
            listener = body.gameObject.AddComponent<CollisionListener>();
        return listener;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(CollisionEnter != null)
        {
            CollisionEnter(collision);
        }
    }

    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionStay(Collision collision)
    {
        if (CollisionStay != null)
        {
            CollisionStay(collision);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (CollisionExit != null)
        {
            CollisionExit(collision);
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (TriggerEnter != null)
        {
            TriggerEnter(collider);
        }
    }

    /// <summary>
    /// 每帧检测
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay(Collider collider)
    {
        if (TriggerStay != null)
        {
            TriggerStay(collider);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (TriggerExit != null)
        {
            TriggerExit(collider);
        }
    }
}
