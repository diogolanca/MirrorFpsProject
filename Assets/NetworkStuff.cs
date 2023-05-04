using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkStuff : NetworkBehaviour
{
    [SerializeField] private GameObject FpsCamera = null, TPMesh = null, TPModelWeapon = null; 
    // Start is called before the first frame update
    void Start()
    {
        FpsCamera.SetActive(isLocalPlayer);
        TPMesh.SetActive(!isLocalPlayer);
        TPModelWeapon.SetActive(!isLocalPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
