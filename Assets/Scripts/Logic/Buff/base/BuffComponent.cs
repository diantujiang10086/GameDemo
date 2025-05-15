public class BuffComponent : Entity, IAwake<Unit, BuffConfig>, IFixedUpdate, IDestory
{
    private Unit owner;
    private BuffConfig buffConfig;
    private BaseBuff buff;

    public BuffConfig Config => buffConfig;
    public void Awake(Unit a, BuffConfig b)
    {
        owner = a;
        buffConfig = b;
    }

    public void Initialize(BaseBuff buff, BuffArguments buffArguments)
    {
        this.buff = buff;
        buff.Initialize(owner, buffConfig, buffArguments);
    }

    public void AddStack(int count = 1)
    {
        buff.AddStack(count);
    }

    public void FixedUpdate(float elaspedTime)
    {
        if (buff.IsDisposed)
        {
            GetParent<BuffManager>().RemoveBuff(buffConfig.id);
            return;
        }
        buff.FixedUpdate(elaspedTime);
    }

    public void Destory()
    {
        buff.Dispose();
    }
}