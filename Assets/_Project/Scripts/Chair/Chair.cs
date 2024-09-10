using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool IsEmpty { get; set; }

    void Awake()
    {
        IsEmpty = true;
    }

}
