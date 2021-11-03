using System;
using System.Collections.Generic;

namespace HotelPriceScout.Data.Model
{

  public class BookingSite
  {
    private readonly string _name;
    private readonly string _type;

    public BookingSite(string name, string type, string url, List<string> hotels)
    {
      try
      {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Url = url ?? throw new ArgumentNullException(nameof(url));
        HotelsList = CreateHotels(hotels);
      }
      catch (ArgumentException e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    public string Name
    {
      get => _name;
      init { if (value != null) _name = value; }
    }

    public string Type
    {
      get => _type;
      init
      {
        if (value is "single" or "multi") _type = value;
        else throw new ArgumentOutOfRangeException(
            $"{nameof(value)} must be either \"single\" or \"multi\".");
      }
    }

        // TODO: List property, containing hotels.
    public IEnumerable<Hotel> HotelsList { get; init; }



    public string Url { get; }

    public void CreatePriceScraper()
    {
      throw new NotImplementedException();
    }

    private IEnumerable<Hotel> CreateHotels(List<string> hotelsStrings)
    {
      List<Hotel> hotels = new List<Hotel>();
      foreach(string hotel in hotelsStrings)
        {
          hotels.Add(new Hotel(hotel));
        }
      return hotels;     
    }
  }
}