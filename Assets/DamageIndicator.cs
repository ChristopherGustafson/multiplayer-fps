using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(destroyOnDelay());         
    }

    private IEnumerator destroyOnDelay()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

}
