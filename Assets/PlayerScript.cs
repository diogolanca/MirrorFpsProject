using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : NetworkBehaviour
{
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartClient();
    }
}
