using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float KnockBackRange = 8f;
    public float KnockBackTime = 1f;
    public Transform CenterTransform;
    public bool IsKnockingBack { get; private set; }

    private Rigidbody2D Rb;

    private void Start() 
    {
        Rb = GetComponent<Rigidbody2D>();
    }
    
    public void Action(GameObject attackerHitBox)
    {
        var direction = (Vector3)CenterTransform.position - attackerHitBox.transform.position;
        Rb.velocity = (Vector2)(direction.normalized * KnockBackRange);
        IsKnockingBack = true;
        StartCoroutine(UnKnockBack());
    }

    private IEnumerator UnKnockBack()
    {
        yield return new WaitForSeconds(KnockBackTime);
        IsKnockingBack = false;
        Debug.Log($"{gameObject.name} : stop knock back");
    }
}
