using System;
using System.IO;
using System.Collections.Generic;

namespace MultiPlexChecker
{
	public class Ms1
	{
		// GUI param
		private static int nScanLine = 3;

		private StreamReader sr;
		public List<Spectrum> Spectra = new List<Spectrum>();
		public string filename;

		public Ms1()
		{
		}

		public List<int> Run()
		{
			sr = new StreamReader (filename);
			string line = "";
			Spectrum temp = null;
			while((line = sr.ReadLine()) != null)
			{
				// tsv like
				if (line.Contains ("S\t"))
				{
					if (temp != null)
						Spectra.Add (temp);
					temp = new Spectrum ();
				}
				// spectrum atributes
				if (line.Contains ("\t")) 
				{
					string[] itens = line.Split ('\t');
					if (itens [0] == "S")
						temp.s = itens [1];
					else if (itens [1] == "RetTime")
						temp.RetTime = Convert.ToDouble(itens [2]);
					else if (itens [1] == "IonInjectionTime")
						temp.IonInjectionTime = Convert.ToDouble(itens [2]);
					continue;
				}

				// spectrum peaks
				string[] peak = line.Split (' ');
				SpectrumItem it = new SpectrumItem ();
				it.Mz = Convert.ToDouble (peak [0]);
				it.Intensity = Convert.ToDouble (peak [1]);
				// parameter to reduce noise
				if (it.Intensity > Spectrum.Threshold) {
					temp.Peaks.Add (it);
					if (it.Intensity > temp.MaxIntensity)
						temp.MaxIntensity = it.Intensity;
				}
			}
			// add the last one
			Spectra.Add (temp);
			sr.Close ();

			// Index with Spectrum that may have multiplex
			List<int> indexReturn = new List<int> ();

			// build info string
			for(int i = 0; i < Spectra.Count; i++)
			{
				Spectrum sp = Spectra[i];
				sp.Run();
				string info = "";
				foreach(KeyValuePair<int,List<Dictionary<int,Envelope>>> env in sp.Envelopes)
				{
					// main peak
					SpectrumItem spItem = sp.Peaks[env.Key];
					int nPossEnvelopes = env.Value.Count;
					string infoEnv = "\nwindow m/z: " + spItem.Mz.ToString();
					infoEnv += " (" + nPossEnvelopes.ToString() + " env";
					if(nPossEnvelopes > 1) infoEnv += "s";
					infoEnv += ")"; 

					//if(spItem.EvenlopesIndexes.Count>0)
					//	info += " enveloped";

					if(nPossEnvelopes > 1)
					{
						Double total = 0;
						List<Double> intensities = new List<double>();
						List<int> charges = new List<int>();
						List<int> isotopes = new List<int>();
						List<Double> probs = new List<double>();
						foreach(Dictionary<int,Envelope> envZ in env.Value)
						{
							// get the greatest intensity
							Double intensity = 0;
							int charge = 1;
							int nIso = 1;
							Double prob = 0;
							foreach(KeyValuePair<int,Envelope> zi in envZ)
							{
								if(zi.Value.Intensity > intensity)
								{
									intensity = zi.Value.Intensity;
									charge = zi.Key;
									nIso = zi.Value.Isotopes;
									prob = zi.Value.prob;
								}
							}
							total += intensity;
							intensities.Add(intensity);
							charges.Add(charge);
							isotopes.Add (nIso);
							probs.Add (prob);
						}

						Dictionary<int,bool> chargesDic = new Dictionary<int, bool> ();
						for(int j = 0; j < intensities.Count; j++)
						{
							if(j % nScanLine == 0)
								infoEnv += "\n  ";
							infoEnv += string.Format("{0:0.00} (z:{1} n:{2} p:{3:0.00%}) / ", 
								intensities[j], charges[j], isotopes[j], probs[j]);
							chargesDic [charges [j]] = true;
						}

						infoEnv = infoEnv.Substring (0, infoEnv.Length - 3);
						if (info.IndexOf (infoEnv) < 0) {
							info += infoEnv;
						}
					}
				}

				if(info != "" && info.IndexOf(") /") > 0)
					indexReturn.Add(i);

				sp.info = info;
			}
			return indexReturn;
		}
	}

	public class Spectrum
	{
		public static int TopPeak = 2;
		public static Double error = 1E-3;
		public static Double FrontWindow = 1.5;
		public static Double BackWindow = 0.5;
		// to cut down noise spectrum
		public static Double Threshold = 0;

		// prob which it should stop searching for isotpes with higher masses
		public static Double DepthSearchProb = 0.3;

		public string info = "";

		public string s;
		public Double RetTime;
		public Double IonInjectionTime;
		public Double MaxIntensity = 0;

		public List<int> TopIndex;
		public List<SpectrumItem> Peaks = new List<SpectrumItem>();

		// Dictionary of peak indexes, representing its window
		// for each window, there will be a list of possible envelopes
		// for each one of the list will have the charge and sum of intensities
		public Dictionary<int,List<Dictionary<int,Envelope>>> Envelopes = 
			new Dictionary<int,List<Dictionary<int,Envelope>>>();

		public void Run()
		{
			TopIndex = new List<int> ();
			for (int i = 0; i < Peaks.Count; i++) 
			{
				if (TopIndex.Count < TopPeak) 
				{
					TopIndex.Add (i);
					continue;
				}

				int minIndex = -1;
				Double minVal = Double.MaxValue;
				for (var j = 0; j < TopIndex.Count; j++) 
				{
					if (Peaks [TopIndex [j]].Intensity < minVal) 
					{
						minVal = Peaks [TopIndex [j]].Intensity;
						minIndex = j;
					}
				}
				if (minVal < Peaks [i].Intensity && minIndex != -1) 
				{
					TopIndex[minIndex] = i;
				}
			}

			// 2.0 Da window analysis for each top spectrum m/z intensity
			for (int j = 0; j < TopIndex.Count; j++) 
			{
				List<SpectrumItem> peakWindow = new List<SpectrumItem> ();
				int index = TopIndex [j];
				Double peakMz = Peaks [index].Mz;
				// back 0.5 Da
				if (index > 0)
					for (var i = index - 1; i >= 0; i--) 
					{
						var mz = Peaks [i].Mz;
						if (mz < peakMz - BackWindow - error)
							break;
						Peaks [i].MainIndex = i;
						peakWindow.Insert (0, Peaks [i]);
					}
				// Foward 1.5 Da
				if(index < Peaks.Count - 1)
					for (var i = index; i < Peaks.Count; i++) 
					{
						var mz = Peaks [i].Mz;
						if (mz > peakMz + FrontWindow + error)
							break;
						Peaks [i].MainIndex = i;
						peakWindow.Add (Peaks [i]);
					}

				// window analysis
				for (var i = 0; i < peakWindow.Count - 1; i++) 
				{
					// n envelopes for each window
					// can be interpreted as the probability to have multiplex sample
					List<Dictionary<int,Envelope>> envelopes = new List<Dictionary<int,Envelope>>();

					Double mz = peakWindow [i].Mz;
					Double intensity = peakWindow [i].Intensity;

					for (var k = i + 1; k < peakWindow.Count - 1; k++) 
					{
						Dictionary<int,Envelope> possEnvelopes = new Dictionary<int, Envelope> ();

						int maxZ = 5;
						if (peakWindow [k].Mz <= mz + 1 / maxZ - error)
							continue;
						// Ion Charges
						for(int z = 2; z < maxZ + 1; z++)
						{
							Double nC = 1;
							Double mzCheck = mz;

							while (mzCheck < peakWindow [k].Mz) 
							{
								Double p1 = Statistics.CalcProb (mz, int.Parse ((nC).ToString()));

								mzCheck += nC / z;
								if (Math.Abs (mzCheck - peakWindow [k].Mz) < error) {
									if (!possEnvelopes.ContainsKey (z))
										possEnvelopes [z] = new Envelope(intensity);
									possEnvelopes [z].Intensity += peakWindow [k].Intensity;
									possEnvelopes [z].Isotopes++;
									possEnvelopes [z].prob = p1;
									peakWindow [k].EvenlopesIndexes.Add (peakWindow [i].MainIndex);
								} 
								else{
									// not all cases we have greater chances to have 
									// firsts isotopes than the last ones (higher multiple)

									//double sumP = 0;
									//for (int nC_Comp = nC - 1; nC_Comp >= 0; nC_Comp--) {
									//	sumP += Statistics.CalcProb (mz, nC_Comp);
									//}

									// prob. to not have this isotope only
									// and to have other higher isotope
									//double pNotHave = 1 - p1 - sumP;

									//if (pNotHave < DepthSearchProb)
										break;
								}
								nC++;
							}
						}
						if(possEnvelopes.Count > 0)
							envelopes.Add (possEnvelopes);
					}

					if (envelopes.Count > 0) 
					{
						if(!Envelopes.ContainsKey(TopIndex[j]))
							Envelopes [TopIndex [j]] = new List<Dictionary<int, Envelope>>();
						Envelopes [TopIndex [j]].AddRange(envelopes);
					}
				}
				
			}
		}
	}

	public class SpectrumItem
	{
		public Double Mz;
		public Double Intensity;

		public int MainIndex;

		//if this spectrum is considered an envelope for other spectrum
		public List<int> EvenlopesIndexes = new List<int>();
	}

	public class Envelope
	{
		public double Intensity;
		public int Isotopes;
		// prob to have this envelope
		public double prob;

		public List<Double> Intensities = new List<double> ();

		// chances to be correct according to statistics analysis
		//public double 

		public Envelope(double intensity){
			Intensity = intensity;
			Isotopes = 1;
		}
	}

}
