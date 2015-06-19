using System;
using System.Collections.Generic;

namespace MultiPlexChecker
{
	public static class Statistics
	{
		// Weighted average mass
		public static Double avgMass = 112.0789;

		// prob n daltons per peptide
		public static Double p0 = 0.937403;
		public static Double p1 = 0.056;
		public static Double p2 = 0.006292;
		public static Double p3 = 0.000286;
		public static Double p4 = 1.486E-5;
		public static Double p5 = 7.85E-7;

		public static Double CalcProb(Double mass, int isotopen)
		{
			Double result = 0;
			Double nPep = mass / avgMass;
			Double compl = nPep - isotopen;

			if (nPep < 0) 
				return 0;

			if (compl < 0)
				return Math.Pow(p1,isotopen);


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

			return result;
		}

		private static Double Combine(Double nPep, int n1, int n2, int n3, int n4, int n5)
		{
			Double res3 = Math.Pow (p1, n1);
			res3 *= Math.Pow (p2, n2);
			res3 *= Math.Pow (p3, n3);
			res3 *= Math.Pow (p3, n4);
			res3 *= Math.Pow (p3, n5);
			int sum = n1 + n2 + n3 + n4 + n5;
			res3 *= Math.Pow (p0, nPep - sum);
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
