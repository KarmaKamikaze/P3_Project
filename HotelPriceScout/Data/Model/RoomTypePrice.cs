using System;

namespace HotelPriceScout.Data.Model
{
  public class RoomTypePrice
  {

    private readonly DateTime _date;
    private decimal _price;

        private static Random random = new Random();

    public RoomTypePrice(DateTime date)
    {
      Date = date;
      Price = random.Next(1000);
    }
    public RoomTypePrice(DateTime date, decimal price) : this(date)
    {
      Price = price;
    }

    public DateTime Date
    {
      get => _date;
      init
      {
        if (value.Date < DateTime.Now.Date)
        {
          throw new ArgumentOutOfRangeException($"{nameof(value)} cannot be a date earlier than the current date.");
        }
        _date = value;
      }
    }

    public decimal Price
    {
      get => _price;
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException($"{nameof(value)} cannot be less than zero.");
        }
        _price = value;
      }
    }
  }
}