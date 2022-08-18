using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public GameObject StartPoint;
    public LayerMask DeathObject;
    
    private bool isDeath;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        if(isDeath)
        {
            Respawn();
        }
    }

    private void CheckDeath()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.position, 0.5f, DeathObject);
        isDeath = colliders.Length > 0;
    }

    public void Respawn()
    {
        player.position = StartPoint.transform.position;
    }
}
