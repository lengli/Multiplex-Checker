using System;
using Gtk;
using System.IO;
using MultiPlexChecker;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	private Ms1 ms1;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
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
		for(int i = 0; i < ms1.Spectra.Count; i++)
		{
			Spectrum sp = ms1.Spectra[i];
			sp.Run();
			string info = "";
			foreach(KeyValuePair<int,List<Dictionary<int,Double>>> env in sp.Envelopes)
			{
				// main peak
				SpectrumItem spItem = sp.Peaks[env.Key];
				int nPossEnvelopes = env.Value.Count;
				info += "\nm/z: " + spItem.Mz.ToString();
				info += " (" + nPossEnvelopes.ToString() + " env";
				if(nPossEnvelopes > 1) info += "s";
				info += ")"; 

				if(spItem.EvenlopesIndexes.Count>0)
					info += " enveloped";

				if(nPossEnvelopes > 1)
				{
					Double total = 0;
					List<Double> intensities = new List<double>();
					List<int> charges = new List<int>();
					foreach(Dictionary<int,Double> envZ in env.Value)
					{
						// get the greatest intensity
						Double intensity = 0;
						int charge = 1;
						foreach(KeyValuePair<int,Double> zi in envZ)
						{
							if(zi.Value > intensity)
							{
								intensity = zi.Value;
								charge = zi.Key;
							}
						}
						total += intensity;
						intensities.Add(intensity);
						charges.Add(charge);
					}
					for(int j = 0; j < intensities.Count; j++)
					{
						if(j % 4 == 0)
							info += "\n  ";
						info += intensities[j].ToString("0.00") + "(" + charges[j].ToString() + ")/";									
					}
					info = info.Substring(0,info.Length - 1);
				}
			}

			if(info != "" && info.IndexOf(")/") > 0)
				mtpxCombo.AppendText(i.ToString());

			sp.info = info;
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
			ms1 = new Ms1 (filechooser.Filename);
			ms1.Run ();
			nSpecLbl.Text = ms1.Spectra.Count.ToString();
			FileNameLbl.Text = filechooser.Filename;
		}

		filechooser.Destroy();
	}

	private void on_dialog_response (object obj, ResponseArgs args)
	{
		Console.WriteLine (args.ResponseId);
	}
}
