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
using System.Collections.Generic;
using Gtk;
using Catalogue;

public partial class CatalogueWindow : Gtk.Window
{	
	private ListStore tModel;
	private FileFilter catalogueFilter;
	private FileFilter htmlFilter;
	
	private enum Context 
	{
		DEFAULT_CONTEXT = 0,
		ADD_CONTEXT,
		REMOVE_CONTEXT
	};
	
	private enum Columns 
	{
		COL_ID = 0,
		COL_TITLE,
		COL_PAGE,
		COL_WORKS
	};

	/// <summary>
	///  Builds up the main Window
	/// </summary>
	public CatalogueWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();

		statusbar.Push ((uint)Context.DEFAULT_CONTEXT, "Welcome to Catalogue " + Utils.GetVersion (true) + " !!");

		this.Icon = new Gdk.Pixbuf (Utils.IconPath ());

		catalogueFilter = new FileFilter ();
		catalogueFilter.AddPattern ("*.catalogue");
		catalogueFilter.Name = "Catalogue files";

		htmlFilter = new FileFilter ();
		htmlFilter.AddPattern ("*.html");
		htmlFilter.AddPattern ("*.htm");
		htmlFilter.Name = "HTML files";


		/* TreeView creation and definition of the sorting algorhithms it'll be using */
		tModel = new ListStore (typeof(int), typeof(string), typeof(int), typeof(bool));

		/* Sort by ID */
		tModel.SetSortFunc ((int)Columns.COL_ID, delegate(TreeModel Model, TreeIter a, TreeIter b)
		{
			int firstVal = (int)Model.GetValue (a, (int)Columns.COL_ID);
			int secondVal = (int)Model.GetValue (b, (int)Columns.COL_ID);

			if (firstVal > secondVal)
				return 1;
			else if (firstVal < secondVal)
				return -1;
			else
				return 0;
		});

		/* Sort by Title */
		tModel.SetSortFunc ((int)Columns.COL_TITLE, delegate(TreeModel Model, TreeIter a, TreeIter b)
		{
			string firstVal = (string)Model.GetValue (a, (int)Columns.COL_TITLE);
			string secondVal = (string)Model.GetValue (b, (int)Columns.COL_TITLE);

			return String.Compare (firstVal, secondVal);
		});

		/* Sort by Page number */
		tModel.SetSortFunc ((int)Columns.COL_PAGE, delegate(TreeModel Model, TreeIter a, TreeIter b)
		{
			int firstVal = (int)Model.GetValue (a, (int)Columns.COL_PAGE);
			int secondVal = (int)Model.GetValue (b, (int)Columns.COL_PAGE);

			if (firstVal > secondVal)
				return 1;
			else if (firstVal < secondVal)
				return -1;
			else
				return 0;
		});

		/* Sort by film status (working films are placed before the non working ones */
		tModel.SetSortFunc ((int)Columns.COL_WORKS, delegate(TreeModel Model, TreeIter a, TreeIter b)
		{
			bool firstVal = (bool)Model.GetValue (a, (int)Columns.COL_WORKS);
			bool secondVal = (bool)Model.GetValue (b, (int)Columns.COL_WORKS);

			if ((firstVal == true) && (secondVal == false))
				return 1;
			else if ((firstVal == false) && (secondVal == true))
				return -1;
			else
				return 0;
		});

		CellRendererText Render = new CellRendererText ()
		{
			Editable = false,
			Ellipsize = Pango.EllipsizeMode.None
		};

		TreeViewColumn ColumnSet;

		mainTreeView.Model = tModel;

		/* Create the four columns and connect the Clicked event to the sorting method */
		mainTreeView.AppendColumn("ID", Render, "text", (int) Columns.COL_ID);
		ColumnSet = mainTreeView.GetColumn((int) Columns.COL_ID);
		ColumnSet.Expand = false;
		ColumnSet.Clickable = true;
		ColumnSet.Clicked += delegate(object sender, EventArgs e)
		{
			SwitchSortMethod (sender, (int) Columns.COL_ID);
		};

		Render = new CellRendererText ()
		{
			Editable = true,
			Ellipsize = Pango.EllipsizeMode.None
		};

		Render.Edited += delegate (object o, EditedArgs args)
		{
			TreeIter iter;
			tModel.GetIterFromString (out iter, args.Path);
			tModel.SetValue (iter, (int) Columns.COL_TITLE, args.NewText);
		};

		mainTreeView.AppendColumn ("Title", Render, "text", (int) Columns.COL_TITLE);
		ColumnSet = mainTreeView.GetColumn ((int) Columns.COL_TITLE);
		ColumnSet.Expand = true;
		ColumnSet.Clickable = true;
		ColumnSet.Clicked += delegate (object sender, EventArgs e)
		{
			SwitchSortMethod (sender, (int) Columns.COL_TITLE);
		};

		Render = new CellRendererText ()
		{
			Editable = true,
			Ellipsize = Pango.EllipsizeMode.None
		};

		Render.Edited += delegate (object o, EditedArgs args)
		{
			TreeIter iter;
			int newPageNum;
			try {
				newPageNum = Int32.Parse (args.NewText);
			}
			catch (Exception e) {
				newPageNum = 0;
				Utils.PrintDebug("UNMANAGED EXCEPTION", e.Message);
			}

			tModel.GetIterFromString (out iter, args.Path);
			tModel.SetValue (iter, (int) Columns.COL_PAGE, newPageNum);
		};

		mainTreeView.AppendColumn ("#Page", Render, "text", (int) Columns.COL_PAGE);
		ColumnSet = mainTreeView.GetColumn ((int) Columns.COL_PAGE);
		ColumnSet.Expand = false;
		ColumnSet.Clickable = true;
		ColumnSet.Clicked += delegate(object sender, EventArgs e)
		{
			SwitchSortMethod (sender, (int) Columns.COL_PAGE);
		};

		CellRendererToggle RenderBool = new CellRendererToggle ()
		{
			Sensitive = true,
			Activatable = true,
		};

		RenderBool.Toggled += delegate (object o, ToggledArgs args)
		{
			TreeIter iter;
			tModel.GetIterFromString (out iter, args.Path);
			tModel.SetValue (iter, (int) Columns.COL_WORKS, (bool) !RenderBool.Active);
		};

		mainTreeView.AppendColumn("Works", RenderBool, "active", (int) Columns.COL_WORKS);
		ColumnSet = mainTreeView.GetColumn ((int) Columns.COL_WORKS);
		ColumnSet.Expand = false;
		ColumnSet.Clickable = true;
		ColumnSet.Clicked += delegate (object sender, EventArgs e)
		{
			SwitchSortMethod (sender, (int) Columns.COL_WORKS);
		};
	}

	/// <summary>
	///  Ask the TreeView to change its sorting method
	/// </summary>
	/// <param name="sender">
	/// A <see cref="System.Object"/> TreeViewColumn (not casted to avoid compatibility issues)
	/// </param>
	/// <param name="column_id">
	/// <see cref="System.Int32"/> the column id
	/// </param>
	private void SwitchSortMethod (object sender, int column_id)
	{
		SortType t;
		if (((TreeViewColumn) sender).SortOrder == SortType.Ascending)
			t = ((TreeViewColumn) sender).SortOrder = SortType.Descending;
		else
			t = ((TreeViewColumn) sender).SortOrder = SortType.Ascending;

		tModel.SetSortColumnId (column_id, t);

		foreach (TreeViewColumn col in mainTreeView.Columns) {
			col.SortIndicator = false;
		}

		((TreeViewColumn) sender).SortIndicator = true;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OpenFileButton (object sender, System.EventArgs e)
	{
		bool open = false;
		FileChooserDialog FChooser = new FileChooserDialog ("Choose a file to open",
		                                                   this, FileChooserAction.Open, this);
		FChooser.AddButton ("Open", ResponseType.Accept);
		FChooser.AddButton ("Cancel", ResponseType.Cancel);
		FChooser.AddFilter (this.catalogueFilter);

		FChooser.SetCurrentFolder (CatalogueParser.GetInstance().DocumentsDir);

		FChooser.Response += delegate (object o, ResponseArgs args) {
			switch (args.ResponseId) {
			case ResponseType.Accept:
				open = true;
				break;
			case ResponseType.Cancel:
				break;
			default:
				break;
			}
		};

		FChooser.Run ();

		if (open)
			LoadFile (FChooser.Filename);


		FChooser.Destroy ();
	}

	public void LoadFile (string fileName)
	{
		tModel.Clear ();
		LinkedList<CatalogueEntry> Entries = CatalogueParser.GetInstance ().OpenFile (fileName);
		foreach (CatalogueEntry entry in Entries) {
			tModel.AppendValues (entry.ID, entry.Title, entry.PageNo, entry.Works);
		}

		Title = "Catalogue - " + CatalogueParser.GetInstance ().OpenedDocumentName;
		statusbar.Push ((uint) Context.DEFAULT_CONTEXT, "Opened file " + CatalogueParser.GetInstance ().OpenedDocumentPath);
	}

	protected virtual void SaveFileButton (object sender, System.EventArgs e)
	{
		string Filename = "";
		TreeIter iter;
		bool save = false;

		if (!CatalogueParser.GetInstance ().Locked || sender.Equals (this.saveAsAction)) {
			FileChooserDialog FChooser = new FileChooserDialog ("Filename to save",
			                                                   this, FileChooserAction.Save, this);
			FChooser.AddButton ("Save", ResponseType.Accept);
			FChooser.AddButton ("Cancel", ResponseType.Cancel);
			FChooser.AddFilter (this.catalogueFilter);
			FChooser.DoOverwriteConfirmation = true;

			FChooser.SetCurrentFolder (CatalogueParser.GetInstance ().DocumentsDir);

			FChooser.Response += delegate(object o, ResponseArgs args) {
				switch (args.ResponseId) {
				case ResponseType.Accept:
					if (FChooser.Filename.Contains (".catalogue"))
						Filename = FChooser.Filename;
					else
						Filename = FChooser.Filename + ".catalogue";

					save = true;
					break;
				case ResponseType.Cancel:
					break;
				default:
					break;
				}
			};

			FChooser.Run ();
			FChooser.Destroy ();
		}

		else {
			save = true;
			Filename = CatalogueParser.GetInstance ().OpenedDocumentPath;
		}

		if (save && (tModel.IterNChildren () > 0)) {
			tModel.GetIterFirst (out iter);
			CatalogueParser Parser = CatalogueParser.GetInstance ();

			Parser.SetCatalogueEntry (new CatalogueEntry(
					(int) tModel.GetValue(iter, (int) Columns.COL_ID),
					(string) tModel.GetValue(iter, (int) Columns.COL_TITLE),
					(int) tModel.GetValue(iter, (int) Columns.COL_PAGE),
					(bool) tModel.GetValue(iter, (int) Columns.COL_WORKS))
				);
			while (tModel.IterNext (ref iter)) {
				Parser.SetCatalogueEntry (new CatalogueEntry(
						(int) tModel.GetValue(iter, (int) Columns.COL_ID),
						(string) tModel.GetValue(iter, (int) Columns.COL_TITLE),
						(int) tModel.GetValue(iter, (int) Columns.COL_PAGE),
						(bool) tModel.GetValue(iter, (int) Columns.COL_WORKS))
					);
			}

			Parser.SaveFileName(Filename);
			Title = "Catalogue - " + Parser.OpenedDocumentName;
			statusbar.Push ((uint) Context.DEFAULT_CONTEXT, "Saved file " + Filename);
		}
	}

	protected virtual void AddLineButton (object sender, System.EventArgs e)
	{
		int page = 0;
		TreeIter lastItem;
		bool got = tModel.IterNthChild (out lastItem, tModel.IterNChildren () - 1);

		if (got)
			page = (int) tModel.GetValue (lastItem, (int) Columns.COL_PAGE);

		TreeIter iter = tModel.AppendValues (CatalogueParser.GetInstance ().UpdateHighestID (),
			"Test", page, true);

		mainTreeView.ScrollToCell (tModel.GetPath (iter),
			mainTreeView.GetColumn ((int) Columns.COL_TITLE),
			true, .0F, .0F);
	}

	protected virtual void DeleteLineButton (object sender, System.EventArgs e)
	{
		TreeIter iter;
		TreeSelection t = mainTreeView.Selection;
		if (t.GetSelected(out iter)) {
			CatalogueParser.GetInstance().DeleteByID((int) tModel.GetValue(iter, (int) Columns.COL_ID));
			tModel.Remove(ref iter);
		}
	}

	protected virtual void AboutButton (object sender, System.EventArgs e)
	{
		AboutDialog AbDialog = new AboutDialog ()
		{
			Icon = new Gdk.Pixbuf (Utils.IconPath ()),
			Logo = new Gdk.Pixbuf (Utils.IconPath ()),
			Version = Utils.GetVersion (false),
			Copyright = "Copyright (C) 2011 - Massimo Gengarelli <gengarel@cs.unibo.it>",
			License = Utils.LICENSE,
			Website = "http://github.com/massix/Catalogue"
		};

		AbDialog.Authors = new string[]
		{
			"Massimo Gengarelli <gengarel@cs.unibo.it>",
			"Drs Alice Chirdo <alicechirdo@gmail.com>"
		};

		AbDialog.Run ();
		AbDialog.Destroy ();
	}

	protected virtual void FindButtonActivate (object sender, System.EventArgs e)
	{
		TreeIter iterFirst, iterLast;
		tModel.GetIterFirst (out iterFirst);
		tModel.IterNthChild (out iterLast, tModel.IterNChildren ());
		try {
			string firstVal = (string) tModel.GetValue (iterFirst, (int) Columns.COL_TITLE);
			if (firstVal.ToLower ().Contains (entry3.Text.ToLower ()))
				mainTreeView.ScrollToCell (tModel.GetPath (iterFirst),
					mainTreeView.GetColumn ((int) Columns.COL_TITLE),
					true, .0F, .0F);
			else {
				while (tModel.IterNext (ref iterFirst)) {
					firstVal = (string) tModel.GetValue (iterFirst, (int) Columns.COL_TITLE);
					if (firstVal.ToLower ().Contains (entry3.Text.ToLower ())) {
						mainTreeView.ScrollToCell (tModel.GetPath (iterFirst),
							mainTreeView.GetColumn ((int) Columns.COL_TITLE),
							true, .0F, .0F);
						break;
					}
				}
			}
		}
		catch (System.NullReferenceException) {
			Dialog errDiag = new Dialog ("Genius", this, DialogFlags.Modal, this);
			errDiag.VBox.Add (new Label ("Ok genius, where am I supposed to search?"));
			errDiag.AddButton ("Sorry, I'm dumb.", ResponseType.Accept);
			errDiag.ShowAll ();
			errDiag.Run ();
			errDiag.Destroy ();
		}
	}

	protected virtual void ExportAction (object sender, System.EventArgs e)
	{
		string filename = "";
		FileChooserDialog FileSave = new FileChooserDialog ("Select a filename to export to", this, FileChooserAction.Save, this);
		FileSave.AddFilter (htmlFilter);
		FileSave.AddButton ("Export", ResponseType.Accept);
		FileSave.AddButton ("Cancel", ResponseType.Cancel);
		FileSave.DoOverwriteConfirmation = true;

		FileSave.SetCurrentFolder (CatalogueParser.GetInstance ().DocumentsDir);

		FileSave.Response += delegate (object o, ResponseArgs args) {
			switch (args.ResponseId) {
			case ResponseType.Accept:
				if (!FileSave.Filename.Contains (".htm"))
					filename = FileSave.Filename + ".html";
				else
					filename = FileSave.Filename;
				break;
			default:
				break;
			}
		};

		FileSave.Run ();
		FileSave.Destroy ();

		try {
			if (filename != "") {
				CatalogueExporter Exporter = new CatalogueExporter (ExportType.TO_HTML);
				Exporter.Export (filename);
				System.Diagnostics.Process.Start (filename);
				statusbar.Push ((uint) Context.DEFAULT_CONTEXT, "Exported catalogue to " + filename);
			}
		}
		catch (System.UnauthorizedAccessException exc) {
			Dialog errDiag = new Dialog ("You don't have the rights to write there!", this, DialogFlags.Modal, this);
			errDiag.VBox.Add (new Label (exc.Message + "\n\nPlease try again with another path"));
			errDiag.AddButton ("Ok, I will. I promise.", ResponseType.Accept);
			errDiag.ShowAll ();
			errDiag.Run ();
			errDiag.Destroy ();
		}
		catch (Exception exc) {
			Dialog errDiag = new Dialog ("Unknown exception", this, DialogFlags.Modal, this);
			errDiag.VBox.Add (new Label (exc.Message));
			errDiag.AddButton ("Dunno, lol :-(", ResponseType.Accept);
			errDiag.ShowAll ();
			errDiag.Run ();
			errDiag.Destroy ();
			Utils.PrintDebug ("XSLT Exception", exc.Message);
		}
	}

	protected virtual void NewActivate (object sender, System.EventArgs e)
	{
		CatalogueParser.GetInstance ().Reset ();
		tModel.Clear ();
		Title = "Catalogue";
	}

	protected virtual void PreferencesActivated (object sender, System.EventArgs e)
	{
		try {
			CataloguePreferences PrefDiag = new CataloguePreferences ()
			{
				Icon = new Gdk.Pixbuf (Utils.IconPath ())
			};

			PrefDiag.Run ();
			PrefDiag.Destroy ();
		}
		catch (CataloguePreferencesException exc) {
			Dialog errDiag = new Dialog ("Error while parsing Exception file", this, DialogFlags.Modal, this);
			errDiag.VBox.Add (new Label (exc.Message +
				"\nProbably you or your friends modified the Preferences file and now everything is messed up.\n" +
				"Delete the Preferences file and DO NOT try to bug me again.\n" +
				"Right, kid?\n\n" +
				"And, I forgot, it's very likely that the application will crash when you'll click the giant \"Save\" button.\n" +
				"See you!"));
			errDiag.AddButton ("Yes, sir!", ResponseType.Accept);
			errDiag.ShowAll ();
			errDiag.Run ();
			errDiag.Destroy ();
		}
	}

	protected virtual void ExitActivate (object sender, System.EventArgs e)
	{
	}
}