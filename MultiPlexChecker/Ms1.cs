using System;
using System.IO;
using System.Collections.Generic;

namespace MultiPlexChecker
{
	public class Ms1
	{
		private StreamReader sr;
		public List<Spectrum> Spectra = new List<Spectrum>();
		public string filename;

		public Ms1()
		{
		}

		public void Run()
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
		public Dictionary<int,List<Dictionary<int,Double>>> Envelopes = 
			new Dictionary<int,List<Dictionary<int,Double>>>();

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
					List<Dictionary<int,Double>> envelopes = new List<Dictionary<int,Double>>();

					Double mz = peakWindow [i].Mz;
					Double intensity = peakWindow [i].Intensity;

					for (var k = i + 1; k < peakWindow.Count - 1; k++) 
					{
						Dictionary<int,Double> possEnvelopes = new Dictionary<int, double> ();

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
								mzCheck += nC / z;
								if (Math.Abs (mzCheck - peakWindow [k].Mz) < error) {
									if (!possEnvelopes.ContainsKey (z))
										possEnvelopes [z] = intensity;
									// give less importance for higher charges(increase probability of noise)
									// less importance to multiple of the envelope
									possEnvelopes [z] += peakWindow [k].Intensity / (z * nC);
									peakWindow [k].EvenlopesIndexes.Add (peakWindow [i].MainIndex);
								} 
								// we have much greater chances to have the firsts isotopes
								// than the last one (higher multiple)
								else
									break;
								nC++;
							}
						}
						if(possEnvelopes.Count > 0)
							envelopes.Add (possEnvelopes);
					}

					if (envelopes.Count > 0) 
					{
						if(!Envelopes.ContainsKey(TopIndex[j]))
							Envelopes [TopIndex [j]] = new List<Dictionary<int, double>>();
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
}
