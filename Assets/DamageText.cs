using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    GameObject cameraGo;

    void DestroyText()
    {
        Destroy(gameObject);
    }

    public void GetCalled(float damage_, GameObject camera_)
    {
        GetComponent<TMP_Text>().text = damage_.ToString();
        cameraGo = camera_;
    }

    private void LateUpdate()
    {
        if (cameraGo != null)
        {
            transform.LookAt(transform.position + cameraGo.transform.rotation * Vector3.forward, cameraGo.transform.rotation * Vector3.up);
        }
    }
}
