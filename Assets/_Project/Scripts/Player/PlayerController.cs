using UnityEngine;

/// <summary>
/// This is instantiated for every player, even online. 
/// Generally you want to calculate inputs inside InputManager attached to this same GameObject, then your pawn in scene will read those inputs
/// </summary>
public class PlayerController : MonoBehaviour
{
    //user
    [SerializeField] User user;
    public User User => user;

    //pawn
    private PlayerPawn currentPawn;
    public PlayerPawn CurrentPawn => currentPawn;

    void Awake()
    {
        //be sure is unparent - set DontDestroyOnLoad
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        //if user is null, try get component
        if (user == null) 
            user = GetComponent<User>();
    }

    /// <summary>
    /// Possess pawn - if already possessing a pawn, it will be unpossessed
    /// </summary>
    /// <param name="pawn"></param>
    public void Possess(PlayerPawn pawn)
    {
        //if this controller has already a pawn, unpossess it
        Unpossess();

        if (pawn)
        {
            //if the new pawn ahs already a controller, call unpossess on it
            pawn.Unpossess();

            //then set pawn and controller 
            currentPawn = pawn;
            currentPawn.CurrentController = this;
            currentPawn.OnPossess(this);
        }
    }

    /// <summary>
    /// Unpossess current pawn
    /// </summary>
    public void Unpossess()
    {
        if (currentPawn)
        {
            //remove pawn and controller
            currentPawn.CurrentController = null;
            currentPawn.OnUnpossess(this);
            currentPawn = null;
        }
    }
}