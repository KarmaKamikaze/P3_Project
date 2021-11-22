﻿using System.Collections.Generic;
using HotelPriceScout.Data.Model;
using System.Linq;
using System;

namespace HotelPriceScout.Pages
{
        public class PriceMeterGenerator
        {
            public static List<Prices> PriceListGenerator(DateTime TodayDate, IEnumerable<MarketPriceModel> MonthData, decimal avgPrice) // generates a test list of hotels and their prices.
            {
                List<Prices> PriceDataList = new();
            PriceDataList.AddRange(from MarketPriceModel item in MonthData
                                   where (item.Date) == TodayDate.Date
                                   select new Prices(item.HotelName, item.Price));
                PriceDataList.Add(new Prices("Gns. Marked", avgPrice));
                PriceDataList.Sort();
                return PriceDataList;
            }
            public static PriceModel MarketFinder(List<PriceModel> list)
            // Finds the market price in the list of prices,
            // This shall not be used if market price comes from somewhere else.
            {
            PriceModel MarketPriceItem = list.Find(list => list.HotelName == "Gns. Marked");
                return MarketPriceItem;
            }
       
    }
}
