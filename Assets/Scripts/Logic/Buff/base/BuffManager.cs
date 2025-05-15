using System.Collections.Generic;
using System.Linq;
public class BuffManager : Entity, IAwake
{
    private Unit owner;
    private Dictionary<int, BuffComponent> buffs = new();
    public void Awake()
    {
        owner = GetParent<Unit>();
    }

    public bool AddBuff(int buffId, BuffArguments args = default)
    {
        var config = ConfigManager.Instance.GetConfig<BuffConfig>(buffId);
        if (config == null)
            return false;

        if (buffs.TryGetValue(buffId, out var existing))
        {
            if (config.canStack)
            {
                existing.AddStack();
                return true;
            }
            else
            {
                existing.Dispose();
            }
        }
        var buff = BuffAgentManager.Instance.GetBuff(config.templateAgent);
        if (buff == null)
            return false;
        var buffComponent = AddChild<BuffComponent, Unit, BuffConfig>(owner, config);
        buffComponent.Initialize(buff, args);
        buffs[buffId] = buffComponent;
        return true;
    }

    public void RemoveBuff(int buffId)
    {
        if (buffs.TryGetValue(buffId, out var comp))
        {
            comp.Dispose();
            buffs.Remove(buffId);
        }
    }

    public void RemoveAllBuffs()
    {
        foreach (var comp in buffs.Values)
            comp.Dispose();
        buffs.Clear();
    }

    public void RemoveBuffsByType(int type)
    {
        foreach (var kvp in buffs.Where(k => k.Value.Config.buffType == type).ToList())
            RemoveBuff(kvp.Key);
    }
}
