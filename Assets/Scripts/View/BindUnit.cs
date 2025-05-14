using UnityEngine;

public class BindUnit : MonoBehaviour
{
    public Unit unit;
    private void Update()
    {
        unit.position = transform.position;
    }
}