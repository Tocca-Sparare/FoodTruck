using UnityEngine;

public class MapPlayer : BasicStateMachine
{
    [SerializeField] GameObject boat;
    [SerializeField] GameObject truck;

    VehicleChanger collidingVeichle;


    public void SwitchVehicle()
    {
        truck.SetActive(!truck.activeSelf);
        boat.SetActive(!boat.activeSelf);
    }

    private void OnTriggerEnter(Collider other)
    {
        var vehicleChanger = other.GetComponentInParent<VehicleChanger>();

        if (vehicleChanger != null && collidingVeichle == null)
        {
            SwitchVehicle();
            collidingVeichle = vehicleChanger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var vehicleChanger = other.GetComponentInParent<VehicleChanger>();

        if (vehicleChanger ==  collidingVeichle)
        {
            collidingVeichle = null;
        }
    }
}
