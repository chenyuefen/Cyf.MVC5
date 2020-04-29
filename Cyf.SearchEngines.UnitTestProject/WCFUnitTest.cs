using System;
using Cyf.SearchEngines.SearchService;
using Cyf.SearchEngines.SearchService.AOP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cyf.SearchEngines.UnitTestProject
{
    [TestClass]
    public class WCFUnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            WCFTest.SearcherClient client = null;
            try
            {
                client = new WCFTest.SearcherClient();
                string result = client.QueryCommodityPage(1, 30, "刘", null, null, null);
                client.Close();
            }
            catch (Exception ex)
            {
                if (client != null)
                    client.Abort();
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            ISearcherAOP searcher = new SearcherAOP();
            var s = searcher.QueryCommodityPage(1, 30, "刘茂", null, null, null);
        }
    }
}
