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
    public static readonly string URL_HTTP = "http://";
    public static readonly string URL_SSP = "ssp://";
    public static readonly string URL_FILE = "file://";

    public static bool IsSourceAvaliable(string url)
    {
        return false;
    }

    public static bool IsSourceAvaliable(string url, InputConfig config)
    {
        return false;
    }

    public static SearchResult SearchSources()
    {
        return new SearchResult();
    }

    public static InputSource Create(string url,GameObject o = null)
    {
        if (url.StartsWith(URL_SSP))
        {
            return new InputSourceSsp(url);
        }
        else
        {
            return new InputSourceStream(url);
        }
    }

    public static void DestroySource(InputSource source)
    {

    }
}
