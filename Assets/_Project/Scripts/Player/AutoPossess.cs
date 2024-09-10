using UnityEngine;

/// <summary>
/// Tell every PlayerController in scene which pawn to control
/// </summary>
public class AutoPossess : MonoBehaviour
{
    [SerializeField] PlayerController playerControllerPrefabForEditorTest;
    [SerializeField] PlayerPawn[] pawnsInOrderByPlayerIndex;

    private void Awake()
    {
        PlayerController[] controllers = GetPlayerControllers();
        PossessPawns(controllers);
    }

    PlayerController[] GetPlayerControllers()
    {
        //get every player controller in scene
        PlayerController[] controllers = FindObjectsByType<PlayerController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        //if there aren't, instantiate prefab
        //(started directly from this scene in editor, or single player without a transition scene where instantiate player controller)
        if (controllers.Length == 0)
        {
            PlayerController controller = Instantiate(playerControllerPrefabForEditorTest);
            controllers = new PlayerController[] { controller };
        }

        return controllers;
    }

    void PossessPawns(PlayerController[] controllers)
    {
        //be sure there are pawns to possess
        if (pawnsInOrderByPlayerIndex == null || pawnsInOrderByPlayerIndex.Length == 0)
        {
            Debug.LogError($"Pawns are null in {gameObject.name} - we use FindObjectsOfType to get them", gameObject);
            pawnsInOrderByPlayerIndex = FindObjectsByType<PlayerPawn>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        }

        //foreach controller, possess a pawn
        foreach (var controller in controllers)
        {
            int index = controller.User.PlayerIndex;
            if (index >= 0 && index < pawnsInOrderByPlayerIndex.Length)
            {
                var pawn = pawnsInOrderByPlayerIndex[index];
                if (pawn != null && pawn.CurrentController == null)
                {
                    pawn.gameObject.SetActive(true);
                    pawn.Possess(controller);
                }
                else
                {
                    Debug.LogError($"Pawn for this index ({index}) is null or already possessed by another PlayerController", gameObject);
                }
            }
            else
            {
                Debug.LogError($"This PlayerController has a wrong index: {index}. Pawns list length is: {pawnsInOrderByPlayerIndex.Length}");
            }
        }
    }
}
