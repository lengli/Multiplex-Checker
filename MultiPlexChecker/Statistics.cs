using System;
using System.Collections.Generic;

namespace MultiPlexChecker
{
	public static class Statistics
	{
		// Weighted average mass
		private static Double avgMass = 112.0789;

		// prob +n daltons per peptide (pn)
		private static List<Double> prob = new List<Double>(){
			0.937403,
			0.056, // 5.6% => prob. to have one isotope
			0.006292,
			0.000286,
			1.486E-5,
			7.85E-7
		};

		// performance prevention
		// key: [m/z]-[n]
		private static Dictionary<string,double> isotopeProbability = new Dictionary<string, double> ();
		// prob. to have a isotope with mass +[isotopen]
		public static Double CalcProb(Double mass, int isotopen)
		{
			string key1 = mass.ToString () + string.Format ("-{0:0}", isotopen);
			if (isotopeProbability.ContainsKey (key1))
				return isotopeProbability [key1];

			Double result = 0;
			Double nPep = mass / avgMass;
			Double compl = nPep - isotopen;

			if (nPep < 0) 
				return 0;

			if (compl < 0)
				return Math.Pow(prob[1],isotopen);


			bool brk = false;
			for (int i5 = 0; i5 < nPep; i5++) 
			{
				for (int i4 = 0; i4 < nPep; i4++) 
				{
					for (int i3 = 0; i3 < nPep; i3++) 
					{
						for (int i2 = 0; i2 < nPep; i2++) 
						{
							int i1 = isotopen - 2 * i2 - 3 * i3 - 4 * i4 - 5 * i5;
							if (i1 < 0) 
							{
								brk = true;
								break;
							}

							result += Combine (nPep, i1, i2, i3, i4, i5);
						}
						if (brk)
							break;
					}
					if (brk)
						break;
				}
				if (brk)
					break;
			}

			isotopeProbability [key1] = result;
			return result;
		}

		private static Double Combine(Double nPep, int n1, int n2, int n3, int n4, int n5)
		{
			Double res3 = Math.Pow (prob[1], n1);
			res3 *= Math.Pow (prob[2], n2);
			res3 *= Math.Pow (prob[3], n3);
			res3 *= Math.Pow (prob[4], n4);
			res3 *= Math.Pow (prob[5], n5);
			int sum = n1 + n2 + n3 + n4 + n5;
			res3 *= Math.Pow (prob[0], nPep - sum);
			res3 *= Combination (sum, nPep);
			return res3;
		}

		//r(r-1)...(r-k+1)/k!
		private static Double Combination(int k, Double r)
		{
			if (k == 0 || r < k)
				return 1;
			if (k < 0)
				return 0;
			
			Double res = 1;
			for(int i = 0; i < k; i++)
			{
				if(r - i > 0)
					res *= r - i;
				if(i != 0 && i != k + 1)
					res /= i;
			}
			return res;
		}

		private static Double Factorial(int n)
		{
			if (n <= 1)
				return 1;
			
			int result = n;
			for (int i = 1; i < n; i++)
				result *= i;
			return result;
		}
	}
}
