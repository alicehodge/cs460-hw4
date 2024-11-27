using HW4.Services;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Globalization;

namespace HW4_Tests
{
    public class Tests
    {
        private TmdbService _tmdbService;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            var logger = new LoggerFactory().CreateLogger<TmdbService>();
            _tmdbService = new TmdbService(httpClient, logger);
        }

        [Test]
        public void TestFormatReleaseDate_ValidDate()
        {
            string releaseDate = "2023-10-15";
            string expected = "October 15, 2023";
            string result = _tmdbService.FormatReleaseDate(releaseDate);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestFormatReleaseDate_InvalidDate()
        {
            string releaseDate = "invalid-date";
            string expected = "Unknown release date";
            string result = _tmdbService.FormatReleaseDate(releaseDate);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestFormatRuntime_LessThanOneHour()
        {
            int runtime = 45;
            string expected = "45 minutes";
            string result = _tmdbService.FormatRuntime(runtime);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestFormatRuntime_OneHour()
        {
            int runtime = 60;
            string expected = "1 hour";
            string result = _tmdbService.FormatRuntime(runtime);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test] 
        public void TestFormatRuntime_OneHourOneMinute()
        {
            int runtime = 61;
            string expected = "1 hour 1 minute";
            string result = _tmdbService.FormatRuntime(runtime);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestFormatRuntime_MoreThanTwoHours()
        {
            int runtime = 125;
            string expected = "2 hours 5 minutes";
            string result = _tmdbService.FormatRuntime(runtime);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestFormatRuntime_ExactHours()
        {
            int runtime = 120;
            string expected = "2 hours";
            string result = _tmdbService.FormatRuntime(runtime);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}