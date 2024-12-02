using System;
using System.Collections.Generic;

namespace foodies_api.model;

public partial class Business
{
    public string Id { get; set; }

    public string ExternalId { get; set; }

    public string Alias { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

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

    public string ImageUrl { get; set; }

    public string Phone { get; set; }

    public virtual ICollection<UserLikeBusiness> UserLikeBusinesses { get; set; } = new List<UserLikeBusiness>();
}
