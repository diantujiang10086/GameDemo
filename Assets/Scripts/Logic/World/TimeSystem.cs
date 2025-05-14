public class TimeSystem : Singleton<TimeSystem>
{
    public float time;
    public float fixedDeltaTime;

    public float GameTime => time;
}