using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text uiAmmo;
    [SerializeField]
    private Slider playerHealthSlider;
    [SerializeField]
    private GameObject damageIndicationPrefab;

    [SerializeField]
    private Canvas UIParent;
    public void SetUIAmmo(int ammo)
    {
        uiAmmo.text = ammo.ToString();
    }

    public void SetUIHealth(float health)
    {
        playerHealthSlider.value = health;
    }

    public void SetDamageIndication(Vector3 from)
    {
        Debug.Log(from);
        Vector3 direction = transform.position - from;
        Debug.Log(direction);
        float angle = Vector3.Angle(transform.forward, direction);
        
        float sign = Mathf.Sign(Vector3.Dot(Vector3.down, Vector3.Cross(transform.forward, direction)));
        angle = angle * sign;
        GameObject damageInd = Instantiate(damageIndicationPrefab, damageIndicationPrefab.transform.position, Quaternion.Euler(0f, 0f, angle));
        damageInd.transform.SetParent(UIParent.transform, false);
    }
}
