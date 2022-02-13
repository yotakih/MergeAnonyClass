using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MergeAnonyClassPrj;
using Odic = System.Collections.Specialized.OrderedDictionary;

namespace MergeAnonyClassPrjTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void AnonyClass2DicTest()
		{
			var a = new
			{
				@class = "hoge fuga piyo",
				@style = "width: 100px; height: 10rem; ",
				@readonly = "",
			};
			var d = MergeAnonyClass.AnonyClass2Dic(a);
			Assert.AreEqual(d.Keys.Count, 3);
		}
		[TestMethod]
		[DataRow("hoge fuga piyo", " ", "", 3, "hoge", "")]
		[DataRow("width: 100px; height: 20px; background-color:red ", ";", ":", 3, "width", "100px")]
		public void AnalizeTest(string trgt, string splt1, string splt2, int crctKyCnt, string tstky, string tstvl)
		{
			var d = MergeAnonyClass.Analize(trgt, splt1, splt2);
			Assert.IsTrue(d.Keys.Count == crctKyCnt && d[tstky].ToString() == tstvl);
		}
		[TestMethod]
		[DataRow(
				new string[] { "k1", "k2", "k3" },
				new string[] { "v1", "v2", "v3" },
				"; ", ":",
				"k1:v1; k2:v2; k3:v3"
		)]
		[DataRow(
				new string[] { "k1", "k2", "k3" },
				new string[] { "", "", "" },
				" ", "",
				"k1 k2 k3"
		)]
		public void ConvOdic2StringTest(string[] klst, string[] vlst, string splt1, string splt2, string crct)
		{
			var dic = new Odic();
			for (int i = 0; i < klst.Length; i++)
				dic[klst[i]] = vlst[i];
			Assert.AreEqual(crct, MergeAnonyClass.ConvOdic2String(dic, splt1, splt2));
		}
		[TestMethod]
		[DataRow(
				new string[] { "enable", "readonly", "class", "style" },
				new string[] { "true", "true", "hoge fuga piyo", "width: 100px; height: 20px;" },
				new string[] { "enable", "hoge", "class", "style" },
				new string[] { "false", "false", "hoge piyo bar", "width: 200px; background-color: red;" },
				new string[] { "enable", "readonly", "hoge", "class", "style" },
				new string[] { "true", "true", "false", "hoge fuga piyo bar", "width:100px; height:20px; background-color:red" }
		)]
		public void MergeTest(string[] k1, string[] v1, string[] k2, string[] v2, string[] crctk, string[] crctv)
		{
			var dic1 = new Odic();
			var dic2 = new Odic();
			for (int i = 0; i < k1.Length; i++) dic1[k1[i]] = v1[i];
			for (int i = 0; i < k2.Length; i++) dic2[k2[i]] = v2[i];
			var retdic = MergeAnonyClass.Merge(dic1, dic2);
			var tmplst = new List<string>();
			foreach (var k in crctk) tmplst.Add(retdic[k].ToString());
			CollectionAssert.AreEqual(crctv, tmplst.ToArray());
		}
	}
}
