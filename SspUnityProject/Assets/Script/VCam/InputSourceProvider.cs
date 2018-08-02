using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputSourceProvider
{
	public class SearchResult : YieldResult<string[]>
	{
		private string[] _result;
		private bool _keepWaiting;

		public override string[] result
		{
			get { return _result; }
		}

		public override bool keepWaiting
		{
			get { return _keepWaiting; }
		}
	}


	public static bool IsSourceAvaliable(string url)
	{
		return false;
	}

	public static bool IsSourceAvaliable(string url, ConfigTable config)
	{
		return false;
	}

	public static SearchResult SearchSources()
	{
		return new SearchResult();
	}

	public static IInputSource Create(string url, GameObject o = null)
	{
		return new InputSource(url);
	}

	public static void DestroySource(IInputSource source)
	{

	}
}
