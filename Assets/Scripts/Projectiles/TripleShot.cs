using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CleanUp());
    }

    IEnumerator CleanUp()
    { 
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

}
