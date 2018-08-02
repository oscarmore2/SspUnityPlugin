using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigTable
{
    protected Dictionary<string, object> properties = new Dictionary<string, object>();

    public Dictionary<string, object> All
    {
        get { return properties; }
    }

    public ConfigTable Add<T>(ConfigProperty<T> pro)
    {
        properties[pro.name] = pro.defaultValue;
        return this;
    }

    public ConfigTable Remove<T>(ConfigProperty<T> pro) 
    {
        if(properties.ContainsKey(pro.name))
        {
            properties.Remove(pro.name);
        }
        return this;
    }

    public ConfigTable Set<T>(ConfigProperty<T> pro, T value)
    {
        properties[pro.name] = value;
        return this;
    }

    public bool Exist<T>(ConfigProperty<T> pro)
    {
        return properties.ContainsKey(pro.name);
    }

    public T Get<T>(ConfigProperty<T> pro)
    {
        if (properties.ContainsKey(pro.name))
            return (T)properties[pro.name];
        return pro.defaultValue;
    }
}
