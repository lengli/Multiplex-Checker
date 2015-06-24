using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		openAction.Activated += OpenFileEvent;
	}

	public void SetNumbSpectLbl(string text)
	{
		nSpecLbl.Text = text;
	}

	public Button RunBtn()
	{
		return runBtn;
	}

	public ComboBox MtpxCombo()
	{
		return mtpxCombo;
	}

	public Label InfoLbl()
	{
		return infoLbl;
	}

	public Label RetLbl()
	{
		return retLbl;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	private void OpenFileEvent(object sender, EventArgs e)
	{
		Dialog dialog = new Dialog
			("Sample", this, Gtk.DialogFlags.DestroyWithParent);
		dialog.Modal = true;
		dialog.AddButton ("Close", ResponseType.Close);
		dialog.Response += new ResponseHandler (on_dialog_response);
		dialog.Run ();
		dialog.Destroy ();
	}

	private void on_dialog_response (object obj, ResponseArgs args)
	{
		Console.WriteLine (args.ResponseId);
	}
}
