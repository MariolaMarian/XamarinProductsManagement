using ProductsManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProductsManagementApp.Converters
{
    public static class ListToObservableConverter<T>
    {
        public static ObservableCollection<T> ToObservable(IEnumerable<T> listToConvert)
        {
            ObservableCollection<T> newCollection = new ObservableCollection<T>();

            if(typeof(T) == typeof(ExpirationDate))
            {
                listToConvert = (IEnumerable<T>)SortExpirationDates((IEnumerable<ExpirationDate>)listToConvert);
            }

            foreach(T item in listToConvert)
            {
                newCollection.Add(item);
            }

            return newCollection;
        }

        private static List<ExpirationDate> SortExpirationDates(IEnumerable<ExpirationDate> oldList)
        {
            var tmp = oldList.ToList();
            tmp.Sort();

            var newList = new List<ExpirationDate>();

            foreach(ExpirationDate exd in tmp)
            {
                if (exd.Count == null)
                    exd.Count = 0;

                if (!exd.Collected)
                {
                    newList.Add(exd);
                }
            }

            foreach (ExpirationDate exd in oldList)
            {
                if (exd.Collected)
                {
                    newList.Add(exd);
                }
            }

            return newList;
        } 
    }
}
