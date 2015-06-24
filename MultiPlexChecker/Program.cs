using System;
using Gtk;
using System.Collections.Generic;

namespace MultiPlexChecker
{
	class MainClass
	{
		// Probability tester
		public static void Main1 (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow();
			win.Title = "Probability Tester";

			VBox vBox = new VBox ();
			TextView massView = new TextView ();
			TextBuffer massBuffer = massView.Buffer;
			TextView isoView = new TextView ();
			TextBuffer isoBuffer = isoView.Buffer;

			Label mLabel = new Label ("Mass");
			Label iLabel = new Label ("n Isotope");
			HBox mBox = new HBox (false,0);
			HBox iBox = new HBox (false,0);

			mBox.Add (mLabel);
			mBox.Add (massView);
			iBox.Add (iLabel);
			iBox.Add (isoView);

			vBox.Add (mBox);
			vBox.Add (iBox);

			Label result = new Label ();
			result.Text = "";
			Button btn = new Button ();
			btn.Clicked += (o, a) => 
			{
				result.Text = "";
				Double mass;
				int iso;
				if(Double.TryParse(massBuffer.Text,out mass) &&
					int.TryParse(isoBuffer.Text,out iso))
				{
					Double prob = Statistics.CalcProb(mass,iso);
					result.Text = prob < 5E-5 ? "< 0.01%" : string.Format("{0:0.00%}", prob);
				}
			};

			Label btnLbl = new Label ("Run");
			btn.Add (btnLbl);
			vBox.Add (btn);
			vBox.Add (result);

			win.Add (vBox);
			win.ShowAll ();
			Application.Run ();
		}

		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow();
			win.Title = "Ms1 Analyzer";

			//Ms1 ms1 = new Ms1 ("121_Cornua_d2_1ul.ms1");
			Ms1 ms1 = new Ms1 ("0.29 - 301.1012.ms1");
			ms1.Run ();

			win.SetNumbSpectLbl("n. of spectrum: " + ms1.Spectra.Count.ToString());

			Label infoLbl = win.InfoLbl();
			Label retTimeLbl = win.RetLbl();

			ComboBox mtpxCombo = win.MtpxCombo();
			ListStore mtpxItems = new ListStore (typeof(string));
			mtpxCombo.Model = mtpxItems;
			mtpxCombo.Changed += (object sender, EventArgs e) => 
			{
				ComboBox combo = sender as ComboBox;
				int index = int.Parse(combo.ActiveText);
				Spectrum sp = ms1.Spectra[index];
				infoLbl.Text = sp.info;
				retTimeLbl.Text = sp.RetTime.ToString();
			};

			win.RunBtn().Clicked += (o, a) => 
			{
				//(mtpxCombo.Model as ListStore).Clear();
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
								if(j % 3 == 0)
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
			};

			win.ShowAll ();
			Application.Run ();
		}
	}
}
