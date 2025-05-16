using UnityEngine;

public class Shape : MonoBehaviour
{
    public IGeometry geometry;
    public virtual void UpdateGeometry()
    {

    }
    public virtual void Draw(Color color)
    {

    }
}
