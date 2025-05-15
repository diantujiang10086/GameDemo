[Buff]
public class CastSkillBuff : BaseBuff
{
    private int skillId;
    private SkillManager skillManager;
    protected override void OnInitialize()
    {
        int.TryParse(config.args0, out skillId);
        skillManager = owner.GetComponent<SkillManager>();
    }

    protected override void OnTick(float elaspedTime)
    {
        skillManager.ExecuteSkill(skillId);
    }
}