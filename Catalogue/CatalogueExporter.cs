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
using System.Xml;
using System.Xml.Xsl;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Catalogue
{
	public enum ExportType
	{
		TO_HTML,
		TO_PDF,
		TO_ODT
	};

	public class CatalogueExporter
	{
		private CatalogueParser Parser;
		private CataloguePreferencesParser Preferences;
		private XslCompiledTransform Transformer;
		private ExportType Type;

		public CatalogueExporter (ExportType Type)
		{
			Parser = CatalogueParser.GetInstance ();
			Preferences = new CataloguePreferencesParser ();
			Transformer = new XslCompiledTransform ();
			this.Type = Type;
		}

		public void Export (string filename)
		{
			switch (Type) {
			case ExportType.TO_PDF:
			case ExportType.TO_ODT:
				Utils.PrintDebug("NOT IMPLEMENTED YET", "ExportType not implemented yet");
				break;
			case ExportType.TO_HTML:
				XmlDocument ToExport = Parser.GetDocument ();
				XsltArgumentList List = new XsltArgumentList ();
				List.AddParam ("PageName", "", System.IO.Path.GetFileNameWithoutExtension (Parser.OpenedDocumentName));
				List.AddParam ("Generated", "", System.DateTime.Today.ToLongDateString ());
				List.AddParam ("SortBy", "", "Page");
				List.AddParam ("imdb_it", "", Preferences.GetPreference ("imdb_it").ToString ());
				List.AddParam ("imdb_univ", "", Preferences.GetPreference ("imdb_univ").ToString ());
				List.AddParam ("google", "", Preferences.GetPreference ("google").ToString ());
				List.AddParam ("google_lang", "", Preferences.GetPreference ("google_lang_str"));
				List.AddParam ("catalogue_version", "", Utils.GetVersion (false));
				Transformer.Load (Utils.ExportHTMLPath ());
				XmlWriter result = new XmlTextWriter (filename, System.Text.Encoding.UTF8);
				Transformer.Transform (ToExport, List, result);
				result.Flush();
				result.Close();
				break;
			}
		}
	}
}

