using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class YieldResult<T> : CustomYieldInstruction
{
    public abstract T result { get; }

    public abstract override bool keepWaiting { get; }
}
