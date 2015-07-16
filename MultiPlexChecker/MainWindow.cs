using System;
using Gtk;
using System.IO;
using MultiPlexChecker;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	private Ms1 ms1;
	private string filename = "";

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		//this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='openAct' action='openAct'/></menu><menu name='AboutAction' action='AboutAction'/></menubar></ui>");
		Build ();
		nSpecLbl.Text = "";
		FileNameLbl.Text = "";
		ErrorLbl.Text = "";

		openAct.Activated += OpenFileEvent;
		mtpxCombo.Changed += MtpxChanged;
		runBtn.Clicked += RunClicked;
	}

	public Button RunBtn()
	{
		return runBtn;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	private void MtpxChanged(object sender, EventArgs e) 
	{
		ComboBox combo = sender as ComboBox;
		int index = int.Parse(combo.ActiveText);
		Spectrum sp = ms1.Spectra[index];
		infoLbl.Text = sp.info;
		retLbl.Text = sp.RetTime.ToString();
	}

	private void RunClicked(object sender, EventArgs e)
	{
		try{
			// Entries validation
			ErrorLbl.Text = "";

			if(LeftWindowEntry.Text != ""){
				double t;
				if (!double.TryParse (LeftWindowEntry.Text.Replace(',' ,'.') , out t)) {
					ErrorLbl.Text = "Insert a valid Real for Left Window Size";
					return;
				}
				Spectrum.BackWindow = t;
			}

			if(RightWindowEntry.Text != ""){
				double t;
				if (!double.TryParse (RightWindowEntry.Text.Replace(',' ,'.') , out t)) {
					ErrorLbl.Text = "Insert a valid Real for Right Window Size";
					return;
				}
				Spectrum.FrontWindow = t;
			}

			if(ThresholdEntry.Text != ""){
				double t;
				if (!double.TryParse (ThresholdEntry.Text.Replace(',' ,'.') , out t)) {
					ErrorLbl.Text = "Insert a valid Real for Threshold";
					return;
				}
				Spectrum.Threshold = t;
			}

			if(TopPeaksEntry.Text != ""){
				int tp;
				if (!int.TryParse (TopPeaksEntry.Text, out tp)) {
					ErrorLbl.Text = "Insert a valid integer on Top Peaks";
					return;
				}
				Spectrum.TopPeak = tp;
			}

			if (filename == "") {
				ErrorLbl.Text = "Choose a file";
				return;			
			}

			ms1 = new Ms1();
			ms1.filename = filename;
			List<int> indexes = ms1.Run ();
			nSpecLbl.Text = ms1.Spectra.Count.ToString();

			mtpxCombo.Clear();
			CellRendererText cell = new CellRendererText();
			mtpxCombo.PackStart(cell, false);
			mtpxCombo.AddAttribute(cell, "text", 0);
			ListStore store = new ListStore(typeof (string));
			mtpxCombo.Model = store;

			for(var i = 0; i < indexes.Count; i++)
				store.AppendValues(indexes[i].ToString());

		}
		catch(Exception ex){
			new Dialog ("Error", this, DialogFlags.Modal,
				"OK",ResponseType.Close);
		}
	}

	private void OpenFileEvent(object sender, EventArgs e)
	{
		Gtk.FileChooserDialog filechooser =
			new Gtk.FileChooserDialog("Choose the file to open",
				this,
				FileChooserAction.Open,
				"Cancel",ResponseType.Cancel,
				"Open",ResponseType.Accept);

		if (filechooser.Run() == (int)ResponseType.Accept) 
		{
			filename = filechooser.Filename;
			FileNameLbl.Text = filename;
		}

		filechooser.Destroy();
	}
}
