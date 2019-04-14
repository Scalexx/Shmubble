using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : MonoBehaviour {

    public float damage;
    public float minSize;

    public float destroyTimer;

    void Update()
    {
        transform.localScale = new Vector3 (Mathf.Lerp(minSize * 2, minSize, destroyTimer), Mathf.Lerp(minSize * 2, minSize, destroyTimer), Mathf.Lerp(minSize * 2, minSize, destroyTimer));
        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

}
