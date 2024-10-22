using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Entities;

public class Business
{
    [Key]
    public string Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
    public string? ImageURL { get; set; }
    public string URL { get; set; }
    public int ReviewCount { get; set; }
    public List<string> Categories { get; set; }
    public int Rating { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
    public string Price { get; set; }
    public string Phone { get; set; }
    public virtual List<UserLikeBusiness> UserLikeBusinesses { get; set; }
}