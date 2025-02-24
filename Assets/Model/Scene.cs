public class Scene : Entity
{
    public static Scene inst = new Scene();

    public static Scene CreateScene()
    {
        var scene = Entity.Create(typeof(Scene)) as Scene;
        return scene;
    }
}