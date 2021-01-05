using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float intensity;
    public float smoothness;

    private Quaternion originRotation;
    private Quaternion targetRotation;
    private void Start()
    {
        originRotation = transform.localRotation;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        targetRotation = originRotation * Quaternion.AngleAxis(-intensity * mouseX, Vector3.up) * Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothness);
    }
}
