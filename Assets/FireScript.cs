using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : NetworkBehaviour
{
    [SerializeField] private GameObject camera = null;
    [SerializeField] private LayerMask playerMask = new LayerMask();
    [SerializeField] private GameObject DamageTextParent = null;
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

                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, playerMask))
                {
                    if (hit.collider.TryGetComponent<HealthScript>(out HealthScript playerHealthScript))
                    {
                        if (playerHealthScript.GetHealth() <= 0f) return;
                        GameObject newDamageTxtParent = Instantiate(DamageTextParent, hit.point, Quaternion.identity);
                        newDamageTxtParent.GetComponentInChildren<DamageText>().GetCalled(25, camera);

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
}
