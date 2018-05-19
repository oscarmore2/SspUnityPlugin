using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigProperty<Value>
{
    public string name { get; protected set; }
    public Value defaultValue { get; protected set; }

    public ConfigProperty(string n,Value d)
    {
        name = n;
        defaultValue = d;
    }
}
public class InputConfig  {
    public static readonly ConfigProperty<float> Exposure = new ConfigProperty<float>("Exposure", 1f);
    public static readonly ConfigProperty<float> Temperature = new ConfigProperty<float>("Temperature", 1f);


    protected Dictionary<string, object> properties = new Dictionary<string, object>();
    public bool ApplyTo(InputSource source)
    {
        return null != source && source.ApplyConfig(this);
    }

    public InputConfig ExtractFrom(InputSource source)
    {
        return source.ExtracConfig();
    }

    public Dictionary<string, object> All
    {
        get { return properties; }
    }
    public InputConfig Set<T>(ConfigProperty<T> pro)
    {
        properties[pro.name] = pro.defaultValue;
        return this;
    }
    public InputConfig Set<T>(ConfigProperty<T> pro, T value)
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
            return (T) properties[pro.name];
        return pro.defaultValue;
    }
}
