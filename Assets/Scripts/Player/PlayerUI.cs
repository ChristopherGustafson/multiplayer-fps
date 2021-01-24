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
    private GameObject playerBody;
    [SerializeField]
    private GameObject scoreboard;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

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
        Vector3 direction = playerBody.transform.position - from;
        float angle = Vector3.Angle(playerBody.transform.forward, direction); 
        float sign = Mathf.Sign(Vector3.Dot(Vector3.down, Vector3.Cross(playerBody.transform.forward, direction)));
        angle = angle * sign;
        
        GameObject damageInd = Instantiate(damageIndicationPrefab, damageIndicationPrefab.transform.position, Quaternion.Euler(0f, 0f, angle));
        damageInd.transform.SetParent(transform, false);
        Destroy(damageInd, 0.9f);
    }

    public void UpdateScoreboard(PlayerInfo[] scores)
    {

    }
}
