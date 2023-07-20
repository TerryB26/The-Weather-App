namespace weatherapp;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Globalization;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class view2 : ContentPage
{
    private const string ApiKey = "f964c9569fmsh9af6aa9ff1282cdp10552bjsned31e4e8f478";
    private const string ApiBaseUrl = "https://weatherapi-com.p.rapidapi.com";
    private string cityName = "London";

    public int valueA;
    public view2(int value)
	{
		InitializeComponent();
        FetchDataAndPopulateTable();
        valueA = value;
    }
   private async void FetchDataAndPopulateTable()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", ApiKey);
                string apiUrl = $"{ApiBaseUrl}/forecast.json?q={cityName}&days=7";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                    WeatherData weatherData = new WeatherData
                    {
                        Location = jsonResponse.location.name,

                        IconSource = jsonResponse.forecast.forecastday[valueA].day.condition.icon,
                        Condition = jsonResponse.forecast.forecastday[valueA].day.condition.text,
                        Temperature = jsonResponse.forecast.forecastday[valueA].day.avgtemp_c,
                        CurrentMaxTemp = jsonResponse.forecast.forecastday[valueA].day.maxtemp_c,
                        CurrentMinTemp = jsonResponse.forecast.forecastday[valueA].day.mintemp_c,
                        date = jsonResponse.forecast.forecastday[valueA].date,
                        Humidity = jsonResponse.forecast.forecastday[valueA].day.avghumidity,
                        Snow = jsonResponse.forecast.forecastday[valueA].day.daily_chance_of_snow,
                        Wind = jsonResponse.forecast.forecastday[valueA].day.maxwind_kph,
                        Rain = jsonResponse.forecast.forecastday[valueA].day.daily_chance_of_rain,
                        UV = jsonResponse.forecast.forecastday[valueA].day.uv,
                        SunRiseSet = jsonResponse.forecast.forecastday[valueA].astro.sunrise + " Sunset:  " + 
                                     jsonResponse.forecast.forecastday[valueA].astro.sunset,
                    };
                    
                    DateTime x_date = DateTime.Parse(weatherData.date);
                    string formattedDate = x_date.ToString("dddd, dd MMMM");
                    x_Date.Text = formattedDate;

                    xCurrentIcon.Source = "http:" + weatherData.IconSource;
                    x_Location.Text = weatherData.Location;
                    x_Current.Text = weatherData.Temperature.ToString() + "°C";
                    x_TempFeel.Text = weatherData.CurrentMaxTemp + "° / " + weatherData.CurrentMinTemp + "°";
                    x_Condition.Text = weatherData.Condition;
                    x_Humid.Text = "Humidity: " + weatherData.Humidity.ToString() + " %";
                    x_Wind.Text = "Wind Speed: " + weatherData.Wind.ToString() + " km/h";
                    x_Rain.Text = "Rain Probability: " + weatherData.Wind.ToString() + " %";
                    x_UV.Text = "UV: " + weatherData.UV.ToString() + "";
                    x_SunDetails.Text = "Sunrise:  " + weatherData.SunRiseSet;
                    x_Snow.Text = "Snow Probability: " + weatherData.Snow.ToString() + " %";
                }
                else
                {
                    Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}