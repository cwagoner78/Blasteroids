using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    
    private void Update()
    {
        if (transform.position.y > 25) Destroy(gameObject);
    }

}
