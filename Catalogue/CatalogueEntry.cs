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

namespace Catalogue
{
	public class CatalogueEntry
	{
		public int ID 
		{
			get; private set;
		}
		
		public string Title
		{
			get; private set;
		}
		
		public int PageNo
		{
			get; private set;
		}
		
		public bool Works
		{
			get; private set;
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		private CatalogueEntry ()
		{
			this.ID = 0;
			this.Title = "";
			this.PageNo = 0;
			this.Works = false;
		}

		/// <summary>
		/// Constructor for CatalogueEntry
		/// </summary>
		/// <param name="ID">
		/// A <see cref="System.Int32"/>, the ID of the Entry
		/// </param>
		/// <param name="Title">
		/// A <see cref="System.String"/>, the Title of the entry
		/// </param>
		/// <param name="PageNo">
		/// A <see cref="System.Int32"/>, the page number of the entry
		/// </param>
		/// <param name="Works">
		/// A <see cref="System.Boolean"/> denoting whether the entry works or not
		/// </param>
		public CatalogueEntry (int ID, string Title, int PageNo, bool Works)
		{
			this.ID = ID;
			this.Title = Title;
			this.PageNo = PageNo;
			this.Works = Works;
		}

		/// <summary>
		/// Convert the object into an XML Node
		/// </summary>
		/// <returns>
		/// An <see cref="XmlDocument"/>: the Entry converted.
		/// </returns>
		public XmlDocument ToXml ()
		{
			XmlDocument Element = new XmlDocument ();
			XmlElement RootNode = Element.CreateElement ("CatalogueEntry");
			Element.AppendChild (RootNode);

			RootNode.SetAttribute ("ID", this.ID.ToString ());
			RootNode.SetAttribute ("PageNo", this.PageNo.ToString ());
			RootNode.SetAttribute ("Works", this.Works.ToString ());

			RootNode.InnerText = this.Title;
			
			return Element;
		}
			
	}
}

