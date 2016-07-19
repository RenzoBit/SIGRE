using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIGRE.Controllers
{
	public static class Halp
	{
		public static bool tieneLetra(this string cad)
		{
			if (string.IsNullOrEmpty(cad))
				return false;
			for (int i = 0; i < cad.Length; i++)
				if (char.IsLetter(cad[i]))
					return true;
			return false;
		}

		public static bool esAlfanumerico(this string cad)
		{
			if (string.IsNullOrEmpty(cad))
				return false;
			int let = 0;
			int num = 0;
			int sim = 0;
			for (int i = 0; i < cad.Length; i++)
			{
				if (char.IsLetter(cad[i]))
					let++;
				else if (char.IsNumber(cad[i]))
					num++;
				else
					sim++;
			}
			return ((let > 0 || num > 0) && sim != cad.Length);
		}

		public static bool esTexto(this string cad)
		{
			if (string.IsNullOrEmpty(cad))
				return false;
			int let = 0;
			int num = 0;
			int sim = 0;
			for (int i = 0; i < cad.Length; i++)
			{
				if (char.IsLetter(cad[i]))
					let++;
				else if (char.IsNumber(cad[i]))
					num++;
				else
					sim++;
			}
			return (num == 0 && sim != cad.Length);
		}

		public static bool esPassword(this string cad)
		{
			if (string.IsNullOrEmpty(cad))
				return false;
			if (cad.Length < 8)
				return false;
			int lma = 0;
			int lmi = 0;
			int num = 0;
			for (int i = 0; i < cad.Length; i++)
			{
				if (char.IsUpper(cad[i]))
					lma++;
				else if (char.IsLower(cad[i]))
					lmi++;
				else if (char.IsNumber(cad[i]))
					num++;
			}
			return (lma > 0 && lmi > 0 && num > 0);
		}

		public static string GetMD5(string str)
		{
			byte[] stream = (MD5CryptoServiceProvider.Create()).ComputeHash((new ASCIIEncoding()).GetBytes(str));
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < stream.Length; i++)
				sb.AppendFormat("{0:x2}", stream[i]);
			return sb.ToString();
		}

		public static string GetPassword()
		{
			int length = 3;
			string[] valid = new string[3] { "abcdefghijklmnopqrstuvwxyz", "0123456789", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };
			StringBuilder res = new StringBuilder();
			Random rnd = new Random();
			for (int i = 0; i < valid.Length; i++)
			{
				length = 3;
				while (0 < length--)
					res.Append(valid[i][rnd.Next(valid[i].Length)]);
			}
			return res.ToString();
		}
	}
}
