using System;

public class ConfigAttribute : BaseAttribute
{
    public Type type;
    public ConfigAttribute(Type type)
    {
        this.type = type;
    }
}