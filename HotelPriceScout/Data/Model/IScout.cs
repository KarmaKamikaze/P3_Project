using System;
using System.Collections.Generic;

namespace HotelPriceScout.Data.Model
{
    public interface IScout
    {
        IEnumerable<BookingSite> BookingSites { get; }
        void StartScout();
        void StopScout();
        void RunComparator(string type);
    }
}