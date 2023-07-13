namespace weatherapp;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;


public class WeatherData
{
    public string Location { get; set; }
    public string Condition { get; set; }
    public string Region { get; set; }
    public double Temperature { get; set; }
    public double CurrentMinTemp { get; set; }
    public double CurrentMaxTemp { get; set; }
    public string[] Dates { get; set; }
    public double[] HiLow { get; set; }
    public string[] Conditions { get; set; }

    public int numberOfElements { get; set; }

    public String LC { get; set; }

}

public class ForecastData
{
    public string Date { get; set; }
    public double MaxTemperature { get; set; }
    public double MinTemperature { get; set; }
    public string Condition { get; set; }
}


public partial class MainPage : ContentPage
{

    private const string ApiKey = "f964c9569fmsh9af6aa9ff1282cdp10552bjsned31e4e8f478";
    private const string ApiBaseUrl = "https://weatherapi-com.p.rapidapi.com";

    public Command Col1TappedCommand { get; private set; }


    public MainPage()
	{
		InitializeComponent();
        FetchDataAndPopulateTable();

        Col1TappedCommand = new Command(OnCol1Tapped);
        BindingContext = this;
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

                       CurrentMaxTemp = jsonResponse.forecast.forecastday[0].day.maxtemp_c,
                       CurrentMinTemp = jsonResponse.forecast.forecastday[0].day.mintemp_c,

                        numberOfElements = jsonResponse.forecast.forecastday.Count,
                     
                    };

                    weatherData.Dates = new string[weatherData.numberOfElements];
                    weatherData.HiLow = new double[weatherData.numberOfElements];
                    weatherData.Conditions = new string[weatherData.numberOfElements];




                    for (int i = 0; i < weatherData.numberOfElements; i++)
                    {
                        

                        weatherData.Dates[i] = jsonResponse.forecast.forecastday[i].date;

                        weatherData.HiLow[i] = jsonResponse.forecast.forecastday[i].day.maxtemp_c;

                        weatherData.Conditions[i] = jsonResponse.forecast.forecastday[i].day.condition.text;

                        DateTime date = DateTime.Parse(weatherData.Dates[i]);
                        string formattedDate = date.ToString("dddd, dd");

                        // Get the label dynamically using FindByName
                        string ColName = $"Col{i + 1}";
                        string aName = $"a{i + 1}";
                        string bName = $"b{i + 1}";

                        var labelObject = FindByName(ColName);
                        var labelObject2 = FindByName(bName);
                        var labelObject3 = FindByName(aName);

                        if (labelObject is Label label)
                        {
                            label.Text = formattedDate;
                        }

                        if (labelObject2 is Label label2)
                        {
                            label2.Text = "H: " + weatherData.HiLow[i].ToString() + "°C       L: " + jsonResponse.forecast.forecastday[i].day.mintemp_c + "°C";
                        }

                        if (labelObject3 is Label label3)
                        {
                            label3.Text = weatherData.Conditions[i];
                        }
                    }



                    Location1.Text = weatherData.Location;
                    Current1.Text = weatherData.Temperature.ToString() + "°C";
                    Region1.Text = weatherData.Region;
                    Condition1.Text = weatherData.Condition;
                    HL1.Text = "H: " + weatherData.CurrentMaxTemp.ToString() + "°C - L: " + weatherData.CurrentMaxTemp.ToString() + "°C";

                    //Col1.Text = weatherData.numberOfElements.ToString();
                 

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

    private void OnCol1Tapped()
    {
        int value = 0;
        Navigation.PushAsync(new NewPage1(value));
    }
    private void OnCol2Tapped()
    {
        int value = 1;
        Navigation.PushAsync(new NewPage1(value));
    }
    private void OnCol3Tapped()
    {
        int value = 2;
        Navigation.PushAsync(new NewPage1(value));
    }
}

