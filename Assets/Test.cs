using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        new IdGenerater();
        var gameEventSystem =  new GameEventSystem();
        gameEventSystem.AddAssembly(typeof(Test).Assembly);

    }
}




