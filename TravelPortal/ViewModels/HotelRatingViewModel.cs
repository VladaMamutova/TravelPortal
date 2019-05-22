using System.Collections.Generic;
using System.Linq;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class HotelRatingViewModel
    {
        public List<KeyValuePair<string, long>> Hotel10Collection { get; }
        public RelayCommand SaveInExcel { get; }

        public HotelRatingViewModel()
        {
            var hotelCollection = MainTables.GetHotelRankCollection();

            List<string> keys = hotelCollection.Select(hotel => hotel.Name).ToList();
            List<long> values = hotelCollection.Select(hotel => hotel.VoucherClount).ToList();

            List<KeyValuePair<string, long>> valueList =
                new List<KeyValuePair<string, long>>();
            for (int i = 0; i < hotelCollection.Count && i < 10; i++)
                valueList.Add(new KeyValuePair<string, long>(keys[i], values[i]));
            valueList.Add(new KeyValuePair<string, long>("Другие", hotelCollection.Sum(hotel => hotel.VoucherClount)));
            Hotel10Collection = valueList;

            SaveInExcel = new RelayCommand(o => MainTables.SaveHotelsRankToExcel(hotelCollection), o => true);
        }
    }
}