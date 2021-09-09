using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    public GameObject[] EnemyList;
    void Start()
    {
        foreach(GameObject enemy in EnemyList)
        {
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            foreach(GameObject enemy in EnemyList)
            {
                enemy.SetActive(true);
            }
            var collider = GetComponent<BoxCollider>();
            collider.enabled = false;
        }
    }
}
