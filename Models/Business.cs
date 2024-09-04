using System.ComponentModel.DataAnnotations.Schema;
using foodies_api.Models.Dtos.Yelp;

[NotMapped]
public class Business
{
    public string Alias { get; set; }
    public string Name { get; set; }
    public string URL { get; set; }
    public int ReviewCount { get; set; }
    public List<ReviewDto> Reviews { get; set; }
    public List<CategoryDto> Categories { get; set; }
    public int Rating { get; set; }
    public CoordinatesDto Coordinates { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Price { get; set; }
}