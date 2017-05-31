using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VinEcom.Oms.Log.Entity;
using VinEcom.Oms.Log.Handler;

namespace VinEcom.Oms.Log.UnitTest
{
    [TestClass]
    public class TestLogger
    {
        [TestMethod]
        public void Test_save_ExceptionLogger()
        {
            try
            {
                string s = "adsfafdasfa";
                Logger.Info(s);
                int zero = 0;
                int a = 1 / zero;
            }
            catch (Exception exception)
            {

                Logger.Error(exception);
            }
        }
        [TestMethod]
        public void Test_Get_ExceptionLogger_by_keyword()
        {
            IExceptionLoggerService logService = new ExceptionLoggerService();
            string keyword = "divide by zero";
            long total;
            var ret = logService.SearchByKeyword(keyword, 0, 100, out total);
            Console.WriteLine(ret.Count);

            var firstOrDefault = ret.FirstOrDefault();
            if (firstOrDefault != null)
                Console.WriteLine(firstOrDefault.Message);
        }
    }
}
