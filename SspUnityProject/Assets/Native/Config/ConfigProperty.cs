using System;
using System.Runtime.InteropServices;
using UnityEngine;
using RenderHeads.Media.AVProDeckLink;

public class ConfigProperty<Value>
{
    public string name { get; protected set; }

    public Value defaultValue { get; protected set; }

    public ConfigProperty(string n, Value d)
    {
        name = n;
        defaultValue = d;
    }
}