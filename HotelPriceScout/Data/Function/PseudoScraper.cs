using System;
using System.Linq;
using System.Timers;
using HotelPriceScout.Data.Model;

namespace HotelPriceScout.Data.Function
{
    public delegate void MissingDataWarning(BookingSite bookingSite);
        
    public class PseudoScraper : IDataScraper
    {
        public event MissingDataWarning SendMissingDataWarning;
        private const int TYPE_ONE_MIN = 900;
        private const int TYPE_ONE_MAX = 1400;
        private const int TYPE_TWO_MIN = 900;
        private const int TYPE_TWO_MAX = 1400;
        private const int TYPE_FOUR_MIN = 1200;
        private const int TYPE_FOUR_MAX = 1700;
        private const int DISCREPANCY_PROBABILITY = 100 / 5;
        private const int PRICE_CHANGE_PROBABILITY = 100 / 15;
        private const int ABOVE_OR_BELOW_MARGIN_PROBABILITY = 100 / 50;
        private const int VARIANCE = 200;
        private const int RUN_SCRAPER_INTERVAL_IN_MINUTES = 30;
        private bool _firstTimeUpdate;
        private readonly Random _random = new();
        private decimal _margin;
        private TimeKeeper _updater;
        
        
        
        public PseudoScraper(BookingSite bookingSite)
        {
            BookingSite = bookingSite ?? throw new NullReferenceException();
        }
        
        public BookingSite BookingSite { get; }

        public void StartScraping(decimal margin)
        {
            _margin = margin;
            // Sets the initial room type prices for all 90 days.
            _firstTimeUpdate = true;
            UpdatePrices();
            ValidatePriceData();
            _firstTimeUpdate = false;

            _updater = new TimeKeeper(RUN_SCRAPER_INTERVAL_IN_MINUTES, UpdatePricesAtInterval);
        }

        private void ValidatePriceData()
        {
            if (BookingSite.HotelsList.Any(hotels =>
                hotels.RoomTypes.Any(roomType => roomType.Prices.Any(roomTypePrice => roomTypePrice.Price == 0))))
                SendMissingDataWarning?.Invoke(BookingSite);
        }

        private void UpdatePricesAtInterval(object sender, ElapsedEventArgs eventArgs)
        {
            UpdatePrices();
        }

        private void UpdatePrices()
        {
            foreach (Hotel hotel in BookingSite.HotelsList.Where(hotel => hotel.Name == "Kompas Hotel Aalborg"))
            {
                foreach (RoomType roomType in hotel.RoomTypes)
                {
                    AssignHostRoomPrices(roomType);
                }
            }
            
            foreach (Hotel hotel in BookingSite.HotelsList.Where(hotel => hotel.Name != "Kompas Hotel Aalborg"))
            {
                foreach (RoomType roomType in hotel.RoomTypes)
                {
                    AssignCompetitorRoomPrices(roomType);
                }
            }
        }

        private void AssignHostRoomPrices(RoomType room)
        {
            switch (room.Capacity)
            {
                case 1:
                    foreach (RoomTypePrice price in room.Prices)
                    {
                        price.Price = _random.Next(TYPE_ONE_MIN, TYPE_ONE_MAX);
                    }
                    break;
                case 2:
                    foreach (RoomTypePrice price in room.Prices)
                    {
                        price.Price = _random.Next(TYPE_TWO_MIN, TYPE_TWO_MAX);
                    }
                    break;
                case 4:
                    foreach (RoomTypePrice price in room.Prices)
                    {
                        price.Price = _random.Next(TYPE_FOUR_MIN, TYPE_FOUR_MAX);
                    }
                    break;
            }
        }

        private void AssignCompetitorRoomPrices(RoomType room)
        {
            switch (room.Capacity)
            {
                case 1:
                    SetPrice(room, _random.Next(TYPE_ONE_MIN, TYPE_ONE_MAX));
                    break;
                case 2:
                    SetPrice(room, _random.Next(TYPE_TWO_MIN, TYPE_TWO_MAX));
                    break;
                case 4:
                    SetPrice(room, _random.Next(TYPE_FOUR_MIN, TYPE_FOUR_MAX));
                    break;
            }
        }

        private void SetPrice(RoomType room, decimal hostPriceType)
        {
            decimal maxPrice = (1 + _margin / 100) * hostPriceType;
            decimal minPrice = _margin >= 100 ? 1 : (1 - _margin / 100) * hostPriceType;
            
            foreach (RoomTypePrice price in room.Prices)
            {
                if (_firstTimeUpdate || CheckOutcome(PRICE_CHANGE_PROBABILITY))
                {
                    if (CheckOutcome(DISCREPANCY_PROBABILITY))
                    {

                        price.Price = CheckOutcome(ABOVE_OR_BELOW_MARGIN_PROBABILITY)
                            ? _random.Next((int) maxPrice, (int) (maxPrice + VARIANCE))
                            : minPrice - VARIANCE > 0 
                            ? _random.Next((int) (minPrice - VARIANCE), (int) minPrice)
                            : 1;
                    }
                    else
                    {
                        price.Price = _random.Next((int) minPrice, (int) maxPrice);
                    }
                }
            }
        }

        private bool CheckOutcome(int weight)
        {
            return _random.Next(100) < weight;
        }
    }
}