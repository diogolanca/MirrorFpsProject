using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(HealthValueChanged))] private float HealthValue = 100f;
    [SerializeField] private TMP_Text HealthText = null;
    [SerializeField] private Slider HealthBar = null;
    [SerializeField] private Animator TpAnimator = null;
    [SerializeField] private GameObject mainFPSCamera = null, AfterDeathCamera = null, TPModelMesh = null;
    [SerializeField] private Movement movementScript = null;
    [SerializeField] private CharacterController controller = null;

    private void Start()
    {
        if (!isLocalPlayer) return;
        HealthText.text = HealthValue.ToString();
        HealthBar.value = HealthValue;
    }

    [Server]
    public void GetDamage(float damage_)
    {
        HealthValue = Mathf.Max(0f, HealthValue - damage_);
    }

    void HealthValueChanged(float oldHealth, float newHealth)
    {
        if (!isLocalPlayer) { return; }

        HealthText.text = HealthValue.ToString();
        HealthBar.value = HealthValue;

        if (newHealth <= 0)
        {
            print("die");
            AfterDeathCamera.SetActive(true);
            mainFPSCamera.SetActive(false);
            TPModelMesh.SetActive(true);
            movementScript.enabled = false;
            controller.enabled = false;
            TpAnimator.SetBool("Die", true);
            TpAnimator.SetBool("Walking", true);
        }
    }

    public float GetHealth()
    {
        return HealthValue;
    }
}
