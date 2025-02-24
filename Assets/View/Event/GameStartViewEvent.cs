using UnityEngine;

[Event]
public class GameStartViewEvent : AEvent<GameStartViewInfo>
{
    protected override void Run(GameStartViewInfo _)
    {
        Log.Debug("Game Start View");
    }
}
