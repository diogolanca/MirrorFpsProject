using Mirror;
using System;
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
    [SerializeField] private GameObject RoundOverPanel = null;
    [SerializeField] private TMP_Text WinnerTxt = null;
    Vector3 startPosition;

    private void Start()
    {
        if (!isLocalPlayer) return;
        startPosition = transform.position;
        HealthText.text = HealthValue.ToString();
        HealthBar.value = HealthValue;
    }

    public void NewRoundCall()
    {
        CmdMaxHealth();
    }

    [Command]
    private void CmdMaxHealth()
    {
        ServerMaxHealth();
    }

    [Server]
    private void ServerMaxHealth()
    {
        HealthValue = 100;
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
            movementScript.enabled = false;
            controller.enabled = false;
            print("die");
            RoundOverPanel.SetActive(true);
            WinnerTxt.text = "You Lost";
            AfterDeathCamera.SetActive(true);
            mainFPSCamera.SetActive(false);
            TPModelMesh.SetActive(true);
            TpAnimator.SetBool("Walking", false);
            TpAnimator.SetBool("Die", true);
            Invoke(nameof(BeginNewRound), 5f);
        }
    }

    public void BeginNewRound()
    {
        RoundOverPanel.SetActive(false);
        NewRoundCall();
        movementScript.enabled = false;
        controller.enabled = false;
        transform.position = startPosition;
        AfterDeathCamera.SetActive(false);
        mainFPSCamera.SetActive(true);
        TPModelMesh.SetActive(false);
        TpAnimator.SetBool("Walking", false);
        TpAnimator.SetBool("Die", false);
        movementScript.enabled = true;
        controller.enabled = true;
    }

    public float GetHealth()
    {
        return HealthValue;
    }
}
