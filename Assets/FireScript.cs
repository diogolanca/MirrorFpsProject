using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireScript : NetworkBehaviour
{
    [SerializeField] private GameObject cam = null;
    [SerializeField] private LayerMask playerMask = new LayerMask();
    [SerializeField] private GameObject DamageTextParent = null;
    [SerializeField] private HealthScript healthScript;
    [SerializeField] private GameObject RoundOverPanel = null;
    [SerializeField] private TMP_Text WinnerTxt = null;
    float lastShootTime = 0f;
    float waitForSecondsBetweenShoots = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (lastShootTime == 0 || lastShootTime + waitForSecondsBetweenShoots < Time.time)
            {
                lastShootTime = Time.time;

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, playerMask))
                {
                    if (hit.collider.TryGetComponent<HealthScript>(out HealthScript playerHealthScript))
                    {
                        if (playerHealthScript.GetHealth() - 25 <= 0) 
                        {
                            RoundOverPanel.SetActive(true);
                            WinnerTxt.text = "You Won!";
                            RoundOver();
                        }

                        if (playerHealthScript.GetHealth() <= 0f) return;
                        GameObject newDamageTxtParent = Instantiate(DamageTextParent, hit.point, Quaternion.identity);
                        newDamageTxtParent.GetComponentInChildren<DamageText>().GetCalled(25, cam);

                        if (isServer)
                        {
                            ServerHit(25, playerHealthScript);
                            return;
                        }

                        CmdHit(25, playerHealthScript);
                    }
                }
            }
        }    
    }

    [Command]
    private void CmdHit(float damage, HealthScript playerHealthScript)
    {
        ServerHit(damage, playerHealthScript);
    }

    [Server]
    private void ServerHit(float damage, HealthScript playerHealthScript)
    {
        playerHealthScript.GetDamage(damage);
    }

    private void RoundOver()
    {
        Invoke(nameof(BeginNewRound), 5f);
    }

    private void BeginNewRound()
    {
        healthScript.BeginNewRound();
    }
}
