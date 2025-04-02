using UnityEngine;

public class InputComponent : Entity, IAwake, IUpdate
{
    private Unit unit;
    public void Awake()
    {
        unit = GetParent<Unit>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float enter))
            {
                var point = ray.GetPoint(enter);
                EventSystem.Instance.Publish(new UnitMove {unitId = unit.InstanceId, position = point });
            }
        }
    }
}