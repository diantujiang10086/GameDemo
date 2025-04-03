using UnityEngine;

public abstract class TestDrawShape : MonoBehaviour
{
    public abstract IShape GetShape();
    public abstract void UpdateShape();
}
