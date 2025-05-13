using UnityEngine;

public class Init : MonoBehaviour
{
    private void Awake()
    {
        Loader.LoadAll();
    }
}
