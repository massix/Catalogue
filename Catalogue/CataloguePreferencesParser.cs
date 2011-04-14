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
using System.Xml;

namespace Catalogue
{
	public class CataloguePreferencesParser
	{
		private string pref_path = Utils.PreferencesFilePath ();
		private XmlDocument pref_xml;
		private XmlElement root_node;

		/// <summary>
		/// Initializes the Preferences Holder
		/// </summary>
		public CataloguePreferencesParser ()
		{
			pref_xml = new XmlDocument ();

			if (!File.Exists (pref_path))
				File.WriteAllText (pref_path, Utils.DEFAULT_PREFERENCES);

			pref_xml.Load (pref_path);
			root_node = (XmlElement) pref_xml.FirstChild;
		}

		/// <summary>
		/// Get the preference which XML name is preferenceName
		/// </summary>
		/// <param name="preferenceName">
		/// /// A <see cref="System.String"/> denoting the XML node name of the preference
		/// </param>
		/// <returns>
		/// A <see cref="System.Object"/> representing the preference itself
		/// </returns>
		public object GetPreference (string preferenceName)
		{
			if (!root_node.HasChildNodes)
				return null;

			foreach (XmlNode node in root_node.ChildNodes) {
				string name = node.Attributes.GetNamedItem ("name").InnerText;
				if (!name.Equals (preferenceName))
					continue;

				Type t = Type.GetType (node.Attributes.GetNamedItem ("type").InnerText);

				if (t == typeof(string))
					return (string)node.InnerText;
				else if (t == typeof(int))
					return (int)Int32.Parse (node.InnerText);
				else if (t == typeof(bool))
					return (bool)Boolean.Parse (node.InnerText);
				else
					throw new CataloguePreferencesException ("Requested parameter has a not known type");
			}

			return null;
		}


		public void SetPreference (string preferenceName, object newValue, Type prefType)
		{
			if (!root_node.HasChildNodes)
				throw new CataloguePreferencesException ("Preferences file not created, something is wrong with your OS configuration");

			foreach (XmlNode node in root_node.ChildNodes) {
				string name = node.Attributes.GetNamedItem ("name").InnerText;
				if (!name.Equals (preferenceName))
					continue;

				if (Type.GetType (node.Attributes.GetNamedItem ("type").InnerText) != prefType)
					throw new CataloguePreferencesException ("Types mismatch in preferences");
				else {
					node.InnerText = newValue.ToString ();
					return;
				}
			}

			XmlElement newPref = pref_xml.CreateElement ("Preference");
			root_node.AppendChild (newPref);

			newPref.SetAttribute ("name", preferenceName);
			newPref.SetAttribute ("type", prefType.ToString ());
			newPref.InnerText = newValue.ToString ();
		}

		public void SaveFile ()
		{
			pref_xml.Save (Utils.PreferencesFilePath ());
		}
	}
}

