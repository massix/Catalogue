/* 
 * Catalogue - a small helper that sorts everything.
 * Copyright (c) 2011 Massimo Gengarelli <gengarel@cs.unibo.it>
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the 'Software'), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.IO;

namespace Catalogue
{
	public class Utils
	{
		public const string LICENSE =
@"Copyright (c) 2011 Massimo Gengarelli <gengarel@cs.unibo.it>

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the 'Software'), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.";

		public const string DEFAULT_PREFERENCES =
@"<CataloguePreferences>
	<Preference name=""imdb_it"" type=""System.Boolean"">True</Preference>
	<Preference name=""imdb_univ"" type=""System.Boolean"">True</Preference>
	<Preference name=""google"" type=""System.Boolean"">True</Preference>
	<Preference name=""google_lang"" type=""System.Int32"">0</Preference>
	<Preference name=""google_lang_str"" type=""System.String"">it</Preference>
</CataloguePreferences>
";
		
		public const string IconName = "catalogue.png";
		public const string ExportHTML = "ExportHTML.xslt";
		public const string PreferencesFile = "Preferences.xml";

		public static string DocumentsDir = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
		public static string DataDir = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);

		public static string IconPath ()
		{
			return (Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, IconName));
		}

		public static string ExportHTMLPath ()
		{
			return (Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, ExportHTML));
		}

		public static string PreferencesFilePath ()
		{
			string cataldir = System.IO.Path.Combine (DataDir, "Catalogue");
			if (!Directory.Exists (cataldir))
				Directory.CreateDirectory (cataldir);

			return Path.Combine (cataldir, PreferencesFile);
		}

		public static string GetVersion (bool shortV)
		{
			if (!shortV)
				return (System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.ToString ());
			else {
				int major = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.Major;
				int minor = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.Minor;
				int build = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.Build;
				return (major + "." + minor + "." + build);
			}
		}

		private Utils ()
		{
		}
	}
}

