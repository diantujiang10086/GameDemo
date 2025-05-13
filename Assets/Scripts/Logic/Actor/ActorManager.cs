using System.Collections.Generic;

public class ActorManager : Singleton<ActorManager>
{
    private Dictionary<int, Actor> actors = new Dictionary<int, Actor>();

    public Actor Create(int configId)
    {
        var actor = new Actor();
        actor.Initialize(GenerateId.Generate(), configId);
        actors[actor.id] = actor;
        EventSystem.Instance.Publish(new NewActor { actorId = actor.id });
        return actor;
    }

    public Actor Get(int actorId) 
    {
        actors.TryGetValue(actorId, out var actor);
        return actor;
    }

    public bool Remove(Actor actor)
    {   
        if(actors.Remove(actor.id))
        {
            actor.Dispose();
        }
        return false;
    }
}

public static class GenerateId
{
    private static int total = 1;
    public static int Generate()
    {
        return total++;
    }
}