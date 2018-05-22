using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRenderProcess : IRenderProcess
{

	public override IRenderProcess CreateRenderProcess ()
	{
		return base.CreateRenderProcess ();
	}

    public override void DoRenderProcess()
    {
		this.ProcessBegin ();

		this.ProcessEnd ();
    }

}
