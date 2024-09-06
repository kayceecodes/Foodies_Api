using foodies_api.Models.Dtos.Yelp;

namespace foodies_api.Models.Dtos;

public class GetBusinessResponse
{
    public string Name { get; set; }
    public string Alias { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
    public string? Image_url { get; set; }
    public bool Is_closed { get; set; }
    public string? Url { get; set; }
    public List<string> Categories { get; set; } = null!;
    public double Rating { get; set; }
    public CoordinatesDto? Coordinates { get; set; }
    public string? Price { get; set; }
    public string? Phone { get; set; } = null!;
}
