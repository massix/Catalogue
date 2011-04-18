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
using System.IO;
using System.Collections.Generic;

namespace Catalogue
{
	public class CatalogueParser
	{
		private static CatalogueParser instance = null;
		private XmlDocument OpenedDocument;
		private XmlElement RootElement = null;
		public string DocumentsDir = Utils.DocumentsDir;
		public bool Locked = false;
		
		public string OpenedDocumentPath
		{
			get; private set;
		}
		
		public string OpenedDocumentName
		{
			get; private set;	
		}
		
		private int HighestID = 0;
		
		
		public int UpdateHighestID ()
		{
			return ++HighestID;	
		}
		
		private CatalogueParser ()
		{
			OpenedDocument = new XmlDocument();
			OpenedDocumentName = "Untitled 1.catalogue";
			OpenedDocumentPath = System.IO.Path.Combine(DocumentsDir, OpenedDocumentName);

			RootElement = OpenedDocument.CreateElement("Catalogue");
			OpenedDocument.AppendChild(RootElement);
			HighestID = 0;
		}

		public XmlDocument GetDocument ()
		{
			return OpenedDocument;
		}

		public static CatalogueParser GetInstance ()
		{
			if (instance == null)
				instance = new CatalogueParser ();
			
			return instance;
		}

		public void Reset ()
		{
			instance = null;
		}
		
		public void SetCatalogueEntry (CatalogueEntry e)
		{
			if (RootElement == null)
				RootElement = OpenedDocument.CreateElement ("Catalogue");
			
			if (e.ID > HighestID)
				HighestID = e.ID;
			
			if (RootElement.ChildNodes.Count > 0) {
				XmlNodeList Elements = RootElement.ChildNodes;
				foreach (XmlNode n in Elements) {
					if (GetIDAttribute (n).Equals (e.ID.ToString ())) {
						XmlElement t = (XmlElement)n;
						t.InnerText = e.Title;
						t.SetAttribute ("Works", e.Works.ToString ());
						t.SetAttribute ("PageNo", e.PageNo.ToString ());
						
						return;
					}
				}
			}
			
			RootElement.AppendChild (OpenedDocument.ImportNode (e.ToXml ().ChildNodes[0], true));
		}

		public LinkedList<CatalogueEntry> GetElements ()
		{
			LinkedList<CatalogueEntry> temp = new LinkedList<CatalogueEntry> ();
			foreach (XmlNode n in RootElement.ChildNodes) {
				int ID = Int32.Parse (GetAttribute (n, "ID"));
				int PageNo = Int32.Parse (GetAttribute (n, "PageNo"));
				bool Works = GetAttribute (n, "Works") == "True" ? true : false;
				string Title = n.InnerText;
				temp.AddLast (new CatalogueEntry (ID, Title, PageNo, Works));
			}

			return temp;
		}
		
		private string GetAttribute (XmlNode n, string Attribute)
		{
			return n.Attributes.GetNamedItem (Attribute).InnerText;
		}
		
		private string GetIDAttribute (XmlNode n)
		{
			return n.Attributes.GetNamedItem ("ID").InnerText;	
		}
		
		public void AppendNode (XmlNode Append)
		{
			RootElement.AppendChild (Append);
		}
		
		public void DeleteCatalogueEntry(CatalogueEntry e)
		{
			XmlNodeList Elements = RootElement.ChildNodes;
			foreach (XmlNode n in Elements) {
				if (GetIDAttribute (n).Equals (e.ID.ToString ())) {
					RootElement.RemoveChild (n);
					return;
				}
			}
		}
		
		public void DeleteByID (int id)
		{
			XmlNodeList Elements = RootElement.ChildNodes;
			foreach (XmlNode n in Elements) {
				if (Int32.Parse (GetIDAttribute (n)) == id) {
					RootElement.RemoveChild (n);
					return;
				}
			}
		}
			
		public void PrintOut ()
		{
			OpenedDocument.Save (Console.Out);	
		}
		
		public void SaveFileName (string filename)
		{
			OpenedDocumentName = System.IO.Path.GetFileName (filename);
			DocumentsDir = System.IO.Path.GetDirectoryName (filename);
			OpenedDocumentPath = filename;
			OpenedDocument.Save (OpenedDocumentPath);
			Locked = true;
		}
		
		public LinkedList<CatalogueEntry> OpenFile (string filename)
		{
			LinkedList<CatalogueEntry> Entries = new LinkedList<CatalogueEntry> ();
			StreamReader reader;
			try {
				reader = new StreamReader (filename);
			} catch (Exception e) {
				Utils.PrintDebug("UNMANAGED EXCEPTION", e.Message);
				return null;
			}
			
			OpenedDocument = new XmlDocument ();
			DocumentsDir = System.IO.Path.GetDirectoryName (filename);
			OpenedDocumentName = System.IO.Path.GetFileName (filename);
			OpenedDocumentPath = filename;
			HighestID = 0;
			
			try {
				OpenedDocument.Load (reader);
				RootElement = (XmlElement)OpenedDocument.FirstChild;
				
				if (RootElement == null)
					throw new XmlException ("Root Element is empty");
				
				Entries.Clear ();
				foreach (XmlNode n in RootElement.ChildNodes) {
					int ID = Int32.Parse (GetAttribute (n, "ID"));
					if (ID > HighestID)
						HighestID = ID;
					int PageNo = Int32.Parse (GetAttribute (n, "PageNo"));
					bool Works = GetAttribute (n, "Works").Equals ("True") ? true : false;
					string Title = n.InnerText;
					
					Entries.AddLast (new CatalogueEntry (ID, Title, PageNo, Works));
				}
				
				Locked = true;
			}
			catch (XmlException e) {
				Utils.PrintDebug("UNMANAGED EXCEPTION", e.Message);
			}
			
			reader.Close ();
			reader.Dispose ();
			
			return Entries;
		}
	}
}

