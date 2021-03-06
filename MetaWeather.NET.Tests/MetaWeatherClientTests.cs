using System;
using System.Threading.Tasks;
using Xunit;

namespace MetaWeather.NET.Tests
{
    public class MetaWeatherClientTests
    {
        [Fact]
        public async Task LocationSearchQuery()
        {
            LocationSearch locationSearch;
            using (var client = new MetaWeatherClient())
            {
                locationSearch = (await client.LocationSearch("london"))[0];
            }
            Assert.Equal("London", locationSearch.Title);
            Assert.Equal("City", locationSearch.LocationType);
            Assert.Equal(44418, locationSearch.WOEID);
            Assert.Equal(51.51, Math.Round(locationSearch.Latitude, 2));
            Assert.Equal(-0.13, Math.Round(locationSearch.Longitude, 2));
        }
        [Fact]
        public async Task LocationSearchLatLong()
        {
            LocationSearch locationSearch;
            using (var client = new MetaWeatherClient())
            {
                locationSearch = (await client.LocationSearch(51.506321, -0.12714))[0];
            }
            Assert.Equal("London", locationSearch.Title);
            Assert.Equal("City", locationSearch.LocationType);
            Assert.Equal(44418, locationSearch.WOEID);
            Assert.Equal(51.51, Math.Round(locationSearch.Latitude, 2));
            Assert.Equal(-0.13, Math.Round(locationSearch.Longitude, 2));
            Assert.True(locationSearch.Distance < 10);
        }
        [Fact]
        public async Task Location()
        {
            Location location;
            using (var client = new MetaWeatherClient())
            {
                location = await client.Location(44418);
            }
            Assert.Equal("London", location.Title);
            Assert.Equal("City", location.LocationType);
            Assert.Equal(51.51, Math.Round(location.Latitude, 2));
            Assert.Equal(-0.13, Math.Round(location.Longitude, 2));
            Assert.True(location.ConsolidatedWeather.Length > 3);
            Assert.NotNull(location.Parent);
            Assert.True(location.Sources.Length > 3);
        }
        [Fact]
        public async Task LocationDay()
        {
            Forecast[] forecasts;
            using(var client = new MetaWeatherClient())
            {
                forecasts = await client.LocationDay(44418, new DateTime(2013, 4, 27));
            }
            Assert.Equal(99, forecasts.Length);
            Assert.Equal(366945, forecasts[0].ID);
            Assert.Equal("Light Rain", forecasts[0].WeatherStateName);
            Assert.Equal("N", forecasts[0].WindDirectionCompass);
            Assert.True(forecasts[0].MinTemperature >= 3 && forecasts[0].MinTemperature < 4);
            Assert.True(forecasts[0].MaxTemperature >= 10 && forecasts[0].MaxTemperature < 11);
            Assert.True(forecasts[0].WindSpeed > 9 && forecasts[0].WindSpeed < 10);
            Assert.Equal(358, forecasts[0].WindDirection);
            Assert.Equal(74, forecasts[0].Humidity);
            Assert.Equal(75, forecasts[0].Predictability);
            Assert.True(forecasts[0].Visibility > 9 && forecasts[0].Visibility < 10);
        }
    }
}
