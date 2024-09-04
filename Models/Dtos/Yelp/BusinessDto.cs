namespace foodies_api.Models.Dtos.Yelp;

public class BusinessDto
{
    public string Alias { get; set; }
    public string Name { get; set; }
    public string URL { get; set; }
    public string? Image_url { get; set; }
    public int Rating { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Price { get; set; }

}
