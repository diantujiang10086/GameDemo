public class ControlActor : Entity
{
    public int actorId { get; private set; }

    public void SetActor(int actorId)
    {
        this.actorId = actorId;
    }
}