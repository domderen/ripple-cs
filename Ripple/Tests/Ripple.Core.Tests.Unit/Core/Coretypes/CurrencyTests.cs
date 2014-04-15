using System;
using System.Globalization;
using Deveel.Math;
using NUnit.Framework;
using Ripple.Core.Core.Coretypes;

namespace Ripple.Core.Tests.Unit.Core.Coretypes
{
    [TestFixture]
    public class CurrencyTests
    {
        [Test]
        public void TestDemurraging()
        {
            const string wtfDemure = "015841551A748AD23FEFFFFFFFEA028000000000";
            Currency currency = Currency.FromString(wtfDemure);
            Currency.Demurrage demurrage = currency.demurrage;
            Assert.AreEqual("XAU", demurrage.IsoCode);
            Assert.AreEqual(0.99999999984D, demurrage.IntrestRate);
            Assert.AreEqual("01/24/2014 02:22:10", demurrage.IntrestStart.Date.ToString(CultureInfo.InvariantCulture));
        }

        //[Test]
        //public void TestDemurragingRate() 
        //{
        //    var amount = new BigDecimal("100");
        //    var factor = new BigDecimal("0.995");
        //    var rate = Currency.Demurrage.CalculateRate(factor, new TimeSpan(365, 0, 0, 0));

        //    Console.WriteLine("The starting amount is: " + amount);
        //    Console.WriteLine("The demurrage factor:   " + factor);
        //    Console.WriteLine("The rate:               " + rate);
        //    Console.WriteLine();

        //    for (int days = 1; days < 366 ; days++) {
        //        BigDecimal reduced = Currency.Demurrage.ApplyRate(amount, rate, new TimeSpan(days, 0, 0, 0));
        //        Console.WriteLine(string.Format("After {0} days is {1}",  days, reduced));
        //    }
        //}
    }
}
