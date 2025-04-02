public class Player : Singleton<Player>, IAwake<Unit>
{
    private Unit unit;
    public void Awake(Unit a)
    {
        unit = a;
    }

    public Unit GetUnit()
    {
        return unit;
    }
}
