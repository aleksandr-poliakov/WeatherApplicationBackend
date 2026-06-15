namespace WeatherForecast.Exception;

public class ValidationException(string message) : System.Exception(message);