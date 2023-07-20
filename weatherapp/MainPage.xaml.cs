namespace weatherapp;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Globalization;

public class WeatherData
{
    public string Location { get; set; }
    public string IconSource { get; set; }
    public string Condition { get; set; }
    public string Region { get; set; }
    public double Temperature { get; set; }
    public double CurrentMinTemp { get; set; }
    public double CurrentMaxTemp { get; set; }
    public string[] Dates { get; set; }
    public double[] HiLow { get; set; }
    public string[] Conditions { get; set; }
    public string date { get; set; }
    public int numberOfElements { get; set; }
    public String LC { get; set; }
    public double Humidity { get; set; }
    public double CloudCover { get; set; }
    public double Wind { get; set; }
    public double Rain { get; set; }
    public double Snow { get; set; }
    public string SunRiseSet { get; set; }

    public double UV { get; set; }

    public string x_Input { get; set; }

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
    private string cityName = "London";
    public Command Col1TappedCommand { get; private set; }

    public MainPage()
	{
		InitializeComponent();
        FetchDataAndPopulateTable();
        BindingContext = this;
    }

    public string userInput;

    public void OnSubmitClicked(object sender, EventArgs e)
    {
        userInput = userInputField.Text;

        if (!string.IsNullOrEmpty(userInput))
        {
            string capitalizedInput = CapitalizeFirstLetter(userInput);
            cityName = capitalizedInput;
            FetchDataAndPopulateTable();
        }
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(input);

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
                        x_Input = cityName,
                        Location = jsonResponse.location.name,
                        Condition = jsonResponse.current.condition.text,
                        IconSource = jsonResponse.current.condition.icon,
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
                            label2.Text = "H: " + weatherData.HiLow[i].ToString() + "°C       L: " + 
                                            jsonResponse.forecast.forecastday[i].day.mintemp_c + "°C";
                        }

                        if (labelObject3 is Label label3)
                        {
                            label3.Text = weatherData.Conditions[i];
                        }
                    }

                    Location1.Text = weatherData.Location;
                    wIcon.Source = "http:" + weatherData.IconSource;
                    Current1.Text = weatherData.Temperature.ToString() + "°C";
                    Region1.Text = weatherData.Region;
                    Condition1.Text = weatherData.Condition;
                    HL1.Text = "H: " + weatherData.CurrentMaxTemp.ToString() + "°C - L: " 
                                     + weatherData.CurrentMinTemp.ToString() + "°C";
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

    private async void OnCol1Tapped(object sender, EventArgs e)
    {
        var commandParameter = ((TappedEventArgs)e).Parameter;
        int value = Convert.ToInt32(commandParameter);
        await Navigation.PushAsync(new view2(value));
    }

    private async void OnCol2Tapped(object sender, EventArgs e)
    {
        var commandParameter = ((TappedEventArgs)e).Parameter;
        int value = Convert.ToInt32(commandParameter);
        await Navigation.PushAsync(new view2(value));
    }
    private async void OnCol3Tapped(object sender, EventArgs e)
    {
        var commandParameter = ((TappedEventArgs)e).Parameter;
        int value = Convert.ToInt32(commandParameter);
        await Navigation.PushAsync(new view2(value));
    }
}

