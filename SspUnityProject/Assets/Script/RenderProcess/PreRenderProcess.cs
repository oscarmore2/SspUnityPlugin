using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRenderProcess : IRenderProcess
{
	public override IRenderProcess CreateRenderProcess ()
	{
		return base.CreateRenderProcess ();
	}

    public override void DoRenderProcess()
    {
        throw new NotImplementedException();
    }
}
