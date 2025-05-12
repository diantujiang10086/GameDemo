using UnityEngine;

public class Init : MonoBehaviour
{
    public Sprite sprite;
    private void Awake()
    {
        print(sprite.rect);
        Loader.LoadAll();
    }
}
