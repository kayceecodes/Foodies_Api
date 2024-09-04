namespace foodies_api.Models.Dtos.Yelp;

public class LocationDto
{
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Zip_code { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? State { get; set; }
    public List<string> Display_address { get; set; } = null!;
}
