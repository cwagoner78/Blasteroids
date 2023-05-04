using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _enemy.IsBug()) _enemy.StartHoming();  
    }
}
