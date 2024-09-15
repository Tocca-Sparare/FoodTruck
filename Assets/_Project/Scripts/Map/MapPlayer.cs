using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : NetworkBehaviour
{
    [SerializeField] GameObject boat;
    [SerializeField] GameObject truck;

    List<Collider> colliders = new List<Collider>();

    void SwitchVehicle()
    {
        bool showBoat = colliders.Count <= 0;

        if (NetworkManager.IsOnline)
        {
            if (NetworkManager.instance.Runner.IsServer) 
                RPC_OnChangeVehicle(showBoat);
        }
        else
        {
            //truck if collide with something, else boat
            truck.SetActive(colliders.Count > 0);
            boat.SetActive(colliders.Count <= 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var vehicleChanger = other.GetComponentInParent<VehicleChanger>();

        if (vehicleChanger != null)
        {
            //add to list
            if (colliders.Contains(other) == false)
                colliders.Add(other);

            //check if switch
            SwitchVehicle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var vehicleChanger = other.GetComponentInParent<VehicleChanger>();

        if (vehicleChanger != null)
        {
            //remove from list
            if (colliders.Contains(other))
                colliders.Remove(other);

            //check if switch
            SwitchVehicle();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnChangeVehicle(bool showBoat, RpcInfo info = default)
    {
        boat.gameObject.SetActive(showBoat);
        truck.gameObject.SetActive(!showBoat);
    }
}
