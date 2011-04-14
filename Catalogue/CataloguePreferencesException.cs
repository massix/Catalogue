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


[System.Serializable]
public class CataloguePreferencesException : System.Exception
{
	private object sender = null;

	/// <summary>
	///  Forbid construction of the object with the default constructor
	/// </summary>
	private CataloguePreferencesException () {}

	/// <summary>
	/// Initializes a new instance of the <see cref="T:CataloguePreferencesException"/> class
	/// </summary>
	/// <param name="message">
	/// A <see cref="System.String"/> denoting the message of the exception
	/// </param>
	public CataloguePreferencesException (string message) : base (message) {}

	/// <summary>
	/// Initializes a new instance of the <see cref="T:CataloguePreferencesException"/> class
	/// </summary>
	/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
	/// <param name="sender">A <see cref="T:System.Object"/> that denotes the object that thrown the exception</param>
	public CataloguePreferencesException (string message, object sender) : base (message)
	{
		this.sender = sender;
	}

	/// <summary>
	/// Returns the object that thrown the exception
	/// </summary>
	/// <returns>
	/// The <see cref="System.Object"/> that thrown the exception
	/// </returns>
	public object GetSender ()
	{
		return sender;
	}
}