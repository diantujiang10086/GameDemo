public class Scene : Entity
{
    private static Scene zeroScene;

    public static Scene ZeroScene
    {
        get
        {
            if (zeroScene != null)
                return zeroScene;
            zeroScene = World.inst.GetChild<Scene>(0);
            if (zeroScene == null)
                zeroScene = World.inst.AddChildWithId<Scene>(0);
            return zeroScene;
        }
    }

    public static Scene CreateScene(long id)
    {
        var scene = World.inst.AddChildWithId<Scene>(id);
        return scene;
    }

    public static Scene GetScene(long id)
    {
        return World.inst.GetChild<Scene>(id);
    }
}
