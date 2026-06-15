namespace WeatherForecast.Exceptions;

public class ValidationException(string message) : System.Exception(message);