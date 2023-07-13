namespace weatherapp;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Security.Cryptography.X509Certificates;

public partial class NewPage1 : ContentPage
{
    private const string ApiKey = "f964c9569fmsh9af6aa9ff1282cdp10552bjsned31e4e8f478";
    private const string ApiBaseUrl = "https://weatherapi-com.p.rapidapi.com";

    int number;
    public NewPage1(int value)
	{
        number = value;
		InitializeComponent();
	}

    private async void FetchDataAndPopulateTable()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", ApiKey);
                string apiUrl = $"{ApiBaseUrl}/forecast.json?q=London&days=7";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                    WeatherData weatherData = new WeatherData
                    {
                        Location = jsonResponse.location.name,
                        Condition = jsonResponse.current.condition.text,
                        Temperature = jsonResponse.current.temp_c,
                        Region = jsonResponse.location.region,

                        CurrentMaxTemp = jsonResponse.forecast.forecastday[number].day.maxtemp_c,
                        CurrentMinTemp = jsonResponse.forecast.forecastday[number].day.mintemp_c,

                        numberOfElements = jsonResponse.forecast.forecastday.Count,

                    };

                    weatherData.Dates = new string[weatherData.numberOfElements];
                    weatherData.HiLow = new double[weatherData.numberOfElements];
                    weatherData.Conditions = new string[weatherData.numberOfElements];

                    locName.Text = weatherData.Location;
                    RegName.Text = weatherData.Region;
                    HL2.Text = "H: " + weatherData.HiLow[number].ToString() + "°C       L: " + jsonResponse.forecast.forecastday[number].day.mintemp_c + "°C";
                    Condition1.Text = weatherData.Conditions[number];





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

}