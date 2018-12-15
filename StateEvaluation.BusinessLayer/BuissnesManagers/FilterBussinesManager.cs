﻿using StateEvaluation.BussinesLayer.Providers;
using System.Collections;
using System.Windows.Controls;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
{
    public class FilterBussinesManager
    {
        private FilterProvider filterProvider = new FilterProvider();

        public IEnumerable Filter(DataGrid dataGrid, object filter)
        {
            var filteredData = filterProvider.Filter(filter);
            dataGrid.ItemsSource = filteredData;
            return filteredData;
        }

        public void Clear(DataGrid dataGrid, object filter)
        {
            filterProvider.Clear(filter);
            var filteredData = filterProvider.Filter(filter);
            dataGrid.ItemsSource = filteredData;
        }
    }
}