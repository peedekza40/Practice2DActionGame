using Constants;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float KnockBackRange = 8f;
    public Transform CenterTransform;
    public bool IsKnockingBack { get; private set; }

    private Transform AttackerTransform;
    private Rigidbody2D Rb;

    private void Start() 
    {
        Rb = GetComponent<Rigidbody2D>();        
    }

    public void Action()
    {
        if(AttackerTransform != null)
        {
            var direction = (Vector3)CenterTransform.position - AttackerTransform.position;
            Rb.velocity = (Vector2)(direction.normalized * KnockBackRange);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.name == GameObjectName.HitBox)
        {
            AttackerTransform = other.transform;
        }
    }
}
