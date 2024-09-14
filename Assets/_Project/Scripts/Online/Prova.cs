using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prova : NetworkBehaviour
{
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        Debug.Log("prova");
    }
}
