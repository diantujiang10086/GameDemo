using Unity.Mathematics;

public class BaseBuff
{
    private bool isAutoDispose;
    private bool isTick;
    private float tickTimer;
    private float durationTimer;
    private int stackCount;
    protected Unit owner;
    protected BuffConfig config;
    protected BuffArguments buffArguments;
    private bool isDisposed = false;
    public bool IsDisposed => isDisposed;
    public int StackCount => stackCount;

    public void Initialize(Unit owner, BuffConfig config, BuffArguments buffArguments)
    {
        stackCount = 1;
        tickTimer = 0;
        durationTimer = 0;
        this.owner = owner;
        this.config = config;
        this.buffArguments = buffArguments;
        isTick = config.tickInterval > 0;
        isAutoDispose = config.duration > 0;
        OnInitialize();
    }

    public void AddStack(int count)
    {
        if (config.canStack)
        {
            stackCount = math.min(stackCount + count, config.maxStacks);
            OnStackChanged();
        }

        if (config.canRefresh)
        {
            durationTimer = 0;
            tickTimer = 0;
        }
    }

    public void FixedUpdate(float elaspedTime)
    {
        if(isTick)
        {
            tickTimer += elaspedTime;
            if (tickTimer >= config.tickInterval)
            {
                tickTimer -= config.tickInterval;
                OnTick(elaspedTime);
            }
        }

        if (isAutoDispose)
        {
            durationTimer += elaspedTime;
            if (durationTimer >= config.duration)
            {
                Dispose();
            }
        }
    }

    public void Dispose()
    {
        if (isDisposed)
            return;
        isDisposed = true;
        OnRemove();
    }

    protected virtual void OnInitialize()
    {

    }

    protected virtual void OnTick(float elaspedTime)
    {

    }

    protected virtual void OnStackChanged()
    {

    }

    protected virtual void OnRemove()
    {

    }
}
