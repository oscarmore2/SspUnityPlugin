#region Header
/*
* The authors disclaim copyright to this source code.
* For more details, see the COPYING file included with this distribution.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson {

/// <summary>
/// Stream-like facility to output JSON text.
/// </summary>
public class JsonWriterRaw : JsonWriter {

    public JsonWriterRaw(TextWriter writer)
    {
		if (writer == null) {
			throw new ArgumentNullException("writer");
		}
		this.TextWriter = writer;
		Init();
	}

	protected override void PutString(string str) {
		Put(string.Empty);
		TextWriter.Write('"');
		int n = str.Length;
		for (int i = 0; i < n; i++) {
			switch (str[i]) {
			case '\n':
				TextWriter.Write("\\n");
				continue;
			case '\r':
				TextWriter.Write("\\r");
				continue;
			case '\t':
				TextWriter.Write("\\t");
				continue;
			case '"':
			case '\\':
				TextWriter.Write('\\');
				TextWriter.Write(str[i]);
				continue;
			case '\f':
				TextWriter.Write("\\f");
				continue;
			case '\b':
				TextWriter.Write("\\b");
				continue;
			}

			TextWriter.Write(str[i]);
		}
		TextWriter.Write('"');
	}

}

}
