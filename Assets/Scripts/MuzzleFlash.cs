using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject fireWeaponEffect;
    public GameObject firePoint;
    Shooting shooting;
    
    bool canFlash = true;

    private void Start()
    {
        shooting = FindObjectOfType<Shooting>();
    }
    void Update()
    {
        float waitTime = shooting.bulletWaitTime;

        if (shooting.isShooting && canFlash)
        {
            GameObject effect = Instantiate(fireWeaponEffect, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            effect.transform.SetParent(this.transform);
            canFlash = false;
            Destroy(effect, 0.4f);
            StartCoroutine(waitTimer());
        }

        IEnumerator waitTimer()
        {
            yield return new WaitForSeconds(waitTime);
            canFlash = true;
        }

    }
}
