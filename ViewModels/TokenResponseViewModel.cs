
namespace WeatherData.ViewModels { 

    public class TokenResponseViewModel { 

        public TokenResponseViewModel() { }

        public string access_token { get; set; }
        public int expiration { get; set; }
        public string refresh_token { get; set; }
    }
}