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
			win.ShowAll ();
			Application.Run ();
		}
	}
}
