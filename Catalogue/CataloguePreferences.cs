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
namespace Catalogue
{
	public partial class CataloguePreferences : Gtk.Dialog
	{
		private CataloguePreferencesParser parser;

		public CataloguePreferences ()
		{
			this.Build ();
			parser = new CataloguePreferencesParser ();

			Response += OnResponse;

			IMDb_it.Active = (bool)parser.GetPreference ("imdb_it");
			IMDb_univ.Active = (bool)parser.GetPreference ("imdb_univ");
			Google.Active = (bool)parser.GetPreference ("google");
			glangbox.Sensitive = (bool)parser.GetPreference ("google");

			glangbox.Active = (int)parser.GetPreference ("google_lang") > glangbox.Model.IterNChildren () - 1 ?
				glangbox.Model.IterNChildren () - 1 : (int)parser.GetPreference ("google_lang");
		}


		protected virtual void IMDb_univToggled (object sender, System.EventArgs e)
		{

		}

		protected virtual void IMDb_itToggled (object sender, System.EventArgs e)
		{

		}

		protected virtual void GoogleToggled (object sender, System.EventArgs e)
		{
			glangbox.Sensitive = Google.Active;
		}

		protected virtual void OnResponse (object sender, Gtk.ResponseArgs args)
		{
			if (!(args.ResponseId == Gtk.ResponseType.Ok))
				return;

			parser.SetPreference ("imdb_it", IMDb_it.Active, typeof(bool));
			parser.SetPreference ("imdb_univ", IMDb_univ.Active, typeof(bool));
			parser.SetPreference ("google", Google.Active, typeof(bool));
			parser.SetPreference ("google_lang_str", glangbox.ActiveText.ToLower ().Substring (0, 2), typeof(string));
			parser.SetPreference ("google_lang", glangbox.Active, typeof(int));

			parser.SaveFile ();
		}
	}
}

