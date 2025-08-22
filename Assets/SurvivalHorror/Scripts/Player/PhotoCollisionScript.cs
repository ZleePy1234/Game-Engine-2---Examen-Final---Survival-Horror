using UnityEngine;
using System.Collections.Generic;
using System;

public class PhotoCollisionScript : MonoBehaviour
{
    bool onTrigger;
    GameObject enemy;
    Vector3 directionToEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            onTrigger = true;
            directionToEnemy = (other.transform.position - transform.position).normalized;
        }
    }
    void Update()
    {
        if (!onTrigger)
        {
            return;
        }
        else
        {
            
            if (Physics.Raycast(transform.position, directionToEnemy, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyBehaviours enemy = hit.collider.gameObject.GetComponent<EnemyBehaviours>();
                    enemy.seenByCamera = true;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            onTrigger = false;
            EnemyBehaviours enemy = other.GetComponent<EnemyBehaviours>();
            enemy.seenByCamera = false;
        }
    }
}
