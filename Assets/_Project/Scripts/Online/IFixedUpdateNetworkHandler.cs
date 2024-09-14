
/// <summary>
/// With new Photon, FixedUpdateNetwork is called only on PlayerController instantiated by NetworkManager. Use this interface to call also on Pawn scripts
/// </summary>
public interface IFixedUpdateNetworkHandler
{
    void FixedUpdateNetwork();
}
