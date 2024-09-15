using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : BasicStateMachine
{
    [SerializeField] GameObject boat;
    [SerializeField] GameObject truck;

    List<Collider> colliders = new List<Collider>();

    void SwitchVehicle()
    {
        //truck if collide with something, else boat
        truck.SetActive(colliders.Count > 0);
        boat.SetActive(colliders.Count <= 0);
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
}
