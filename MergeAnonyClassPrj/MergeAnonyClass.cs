using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Odic = System.Collections.Specialized.OrderedDictionary;

namespace MergeAnonyClassPrj
{
	public class MergeAnonyClass
	{
		public static Odic AnonyClass2Dic(object annyCls)
		{
			var retdic = new Odic();
			foreach (var pinf in annyCls.GetType().GetProperties())
			{
				retdic[pinf.Name] = pinf.GetValue(annyCls);
			}
			return retdic;
		}
		public static Odic Analize(string trgt, string splt1, string splt2)
		{
			var retdic = new Odic();
			foreach (var exp in trgt.Split(splt1))
			{
				if (exp.Trim() == "") continue;
				var kyVlLst = exp.Split(splt2);
				if (kyVlLst.Length > 0 && kyVlLst[0].Trim() != "")
				{
					var ky = kyVlLst[0];
					var vl = "";
					if (kyVlLst.Length > 1) vl = kyVlLst[1];
					retdic[ky.Trim()] = vl.Trim();
				}
			}
			return retdic;
		}
		public static Odic Merge(Odic dic1, Odic dic2)
		{
			var kyLst = new List<string>()
						{
								"class",
								"style",
						};
			foreach (var ky in dic2.Keys)
			{
				if (!dic1.Contains(ky))
				{
					dic1[ky] = dic2[ky];
				}
				else
				{
					if (kyLst.Contains(ky.ToString()))
					{
						var splt1 = ky.ToString() == "style" ? ";" : " ";
						var splt2 = ky.ToString() == "style" ? ":" : "";
						var joinsplt = ky.ToString() == "style" ? "; " : " ";
						dic1[ky] = ConvOdic2String(Merge(Analize(dic1[ky].ToString(), splt1, splt2), Analize(dic2[ky].ToString(), splt1, splt2)), joinsplt, splt2);
					}
				}
			}
			return dic1;
		}
		public static string ConvOdic2String(Odic dic, string splt1, string splt2)
		{
			var lst = new List<string>();
			foreach (var ky in dic.Keys)
			{
				lst.Add($"{ky}{splt2}{dic[ky]}");
			}
			return String.Join(splt1, lst);
		}
	}
}
