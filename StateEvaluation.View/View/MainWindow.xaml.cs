﻿using Microsoft.Office.Interop.Excel;
using StateEvaluation.Repository.Providers;
using StateEvaluation.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;
using Window = System.Windows.Window;
using StateEvaluation.Repository.Models;
using StateEvaluation.BussinesLayer.BuissnesManagers;
using StateEvaluation.BioColor.Providers;
using StateEvaluation.BioColor.Helpers;
using StateEvaluation.BusinessLayer.BuissnesManagers;
using Application = System.Windows.Application;

namespace StateEvaluation
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow(new DataRepository());
            MainWindow.ShowDialog();
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const float MAX = 0xFF;
      
        public List<string> PersonCodes = new List<string>();
        public PeopleBuissnesManager peopleBuissnesManager;
        public PreferenceBuissnesManager preferenceBuissnesManager;
        public SubjectiveFeelingBuissnesManager subjectiveFeelingBuissnesManager;
        private PreferenceFilterVM preferenceFilter;
        private PeopleFilterVM peopleFilter;
        private SubjectiveFeelingFilterVM subjectiveFeelingFilter;
        private FilterBussinesManager filterBussinesManager;
        private DataRepository dataRepository;
     
        #region ctor
        public MainWindow(DataRepository dataRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            this.dataRepository = dataRepository;

            biocolorSettings = new BiocolorSettings();

            biocolorProvider = new BiocolorProvider(biocolorSettings);
            imageGenerator = new ImageGenerator(biocolorSettings);

            biocolorProvider.InitBiocolor(BioColorGrid, Date, DateNow);

            filterBussinesManager = new FilterBussinesManager(dataRepository);

            peopleFilter = (PeopleFilterVM)Resources["peopleFilterVM"];
            preferenceFilter = (PreferenceFilterVM)Resources["preferenceFilterVM"];
            subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)Resources["subjectiveFeelingFilterVM"];


            peopleBuissnesManager = new PeopleBuissnesManager
                (dataRepository,
                    new List<ComboBox>() { UserIdsFilterPeopleCB, UserIdsFilterSubjFeelingCB, UserIdsFilterPreferenceCB, UserIdsInsertPreferenceCB, UserIdsInsertSubjFeelCB },
                    new List<ComboBox>() { ExpeditionFromFilterPeopleCB, ExpeditionToFilterPeopleCB, ExpeditionFromFilterSubjFeelCB, ExpeditionToFilterSubjFeelCB, ExpeditionFilterToPreferenceCB, ExpeditionFromFilterPreferenceCB },
                    new List<ComboBox>() { NumberFromFilterPeopleCB, NumberToFilterPeopleCB, NumberFromFilterSubjFeelCB, NumberToFilterSubjFeelCB, NumberFromFilterPreferenceCB, NumberToFilterPreferenceCB },
                    PeopleDataGrid, UpdatePersonBtn
                );

            biocolorBusinessManager = new BiocolorBusinessManager(BioColorGrid, Date, DateNow, biocolorSettings);

            preferenceBuissnesManager = new PreferenceBuissnesManager(dataRepository, PreferencesDataGrid, UpdatePrefernceBtn);

            subjectiveFeelingBuissnesManager = new SubjectiveFeelingBuissnesManager(dataRepository, SubjectiveFeelingDataGrid, UpdateSubjectiveFeelingBtn);
            
            colors = new TextBox[] { ic1, ic2, ic3, ic4, ec1, ec2, ec3, ec4, pc1, pc2, pc3, pc4 };
        }
        #endregion

        #region import
        private void AddData_OnClick(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();

            string filePath = "C:\\Users\\Vitalii.Kushch\\Desktop\\диплом\\Рома\\Шаблон для ежемесячной отправки.xlsx";


            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet sh = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets["Кольорова преференція початок"];

            Range excelRange = sh.UsedRange;

            int i0 = 7, j0 = 45;

            Range xlRng = sh.UsedRange;
            Object arr = xlRng.Value;


            #region MyRegion

            List<List<Byte>> lessSequences1 = new List<List<byte>>();
            List<List<Byte>> Sequences1 = new List<List<byte>>();
            List<string> pref1 = new List<string>();
            List<List<Byte>> lessSequences2 = new List<List<byte>>();
            List<List<Byte>> Sequences2 = new List<List<byte>>();
            List<string> pref2 = new List<string>();
            List<string> number = new List<string>();
            List<string> dates = new List<string>();
            List<string> favcolor = new List<string>();
            List<string> tabl1 = new List<string>();
            List<string> tabl2 = new List<string>();
            List<string> fornames = new List<string>();
            for (int i = i0; i < i0 + 12; ++i)
            {
                fornames.Add(xlRng[i, 2].Value2);
            }
            for (int i = i0; i < xlRng.Rows.Count; ++i)
            {
                tabl1.Add(xlRng[i, 44].Value2.ToString());
                tabl2.Add(xlRng[i, 45].Value2.ToString());
                favcolor.Add(xlRng[i, 10].Value2.ToString());
                dates.Add(xlRng[i, 3].Text.ToString());
                number.Add(xlRng[i, 1].Value2.ToString());
                //dates.Add(((DateTime)xlRng[i, 3]).Date.ToShortDateString());
                lessSequences1.Add(new List<byte>() { (byte)xlRng[i, 12].Value2, (byte)xlRng[i, 13].Value2, (byte)xlRng[i, 14].Value2 });
                Sequences1.Add(new List<byte>() { (byte)xlRng[i, 16].Value2, (byte)xlRng[i, 17].Value2, (byte)xlRng[i, 18].Value2, (byte)xlRng[i, 19].Value2,
                    (byte)xlRng[i, 20].Value2, (byte)xlRng[i, 21].Value2, (byte)xlRng[i, 22].Value2, (byte)xlRng[i, 23].Value2,
                    (byte)xlRng[i, 24].Value2, (byte)xlRng[i, 25].Value2, (byte)xlRng[i, 26].Value2, (byte)xlRng[i, 27].Value2});
                if (xlRng[i, 28].Value2 == null)
                {
                    lessSequences2.Add(null);
                    Sequences2.Add(null);
                    continue;
                }
                lessSequences2.Add(new List<byte>() { (byte)xlRng[i, 28].Value2, (byte)xlRng[i, 29].Value2, (byte)xlRng[i, 30].Value2 });
                Sequences2.Add(new List<byte>() { (byte)xlRng[i, 32].Value2, (byte)xlRng[i, 33].Value2, (byte)xlRng[i, 34].Value2, (byte)xlRng[i, 35].Value2,
                    (byte)xlRng[i, 36].Value2, (byte)xlRng[i, 37].Value2, (byte)xlRng[i, 38].Value2, (byte)xlRng[i, 39].Value2,
                    (byte)xlRng[i, 40].Value2, (byte)xlRng[i, 41].Value2, (byte)xlRng[i, 42].Value2, (byte)xlRng[i, 43].Value2});
            }
            List<StateEvaluationDLL.DataStructures.Preference> prefRes1 = new List<StateEvaluationDLL.DataStructures.Preference>();
            List<StateEvaluationDLL.DataStructures.Preference> prefRes2 = new List<StateEvaluationDLL.DataStructures.Preference>();
            for (var i = 0; i < Sequences1.Count; i++)
            {
                prefRes1.Add(new StateEvaluationDLL.DataStructures.Preference(lessSequences1[i], Sequences1[i]));
            }
            for (var i = 0; i < Sequences2.Count; i++)
            {
                if (Sequences2[i] == null)
                {
                    prefRes2.Add(null);
                    continue;
                }
                prefRes2.Add(new StateEvaluationDLL.DataStructures.Preference(lessSequences2[i], Sequences2[i]));
            }

            foreach (var preference in prefRes1)
            {
                pref1.Add(preference.Type.ToString());
            }
            foreach (var preference in prefRes2)
            {
                if (preference == null)
                {
                    pref2.Add(null);
                    continue;
                }
                pref2.Add(preference.Type.ToString());
            }
            List<Preference> prefinTable = new List<Preference>();
            for (var index = 0; index < pref1.Count; index++)
            {
                var preference = pref1[index];
                string shorder1 = string.Empty;
                string shorder2 = string.Empty;
                string order1 = string.Empty;
                string order2 = string.Empty;
                foreach (var od in lessSequences1[index])
                {
                    shorder1 += od + ",";
                }
                shorder1 = shorder1.Remove(shorder1.Length - 1);
                if (lessSequences2[index] != null)
                {
                    foreach (var od in lessSequences2[index])
                    {
                        if (od == null) break;
                        shorder2 += od + ",";
                    }
                    shorder2 = shorder2.Remove(shorder2.Length - 1);
                }
                foreach (var od in Sequences1[index])
                {
                    order1 += od + ",";
                }
                order1 = order1.Remove(order1.Length - 1);
                if (Sequences2[index] != null)
                {
                    foreach (var od in Sequences2[index])
                    {
                        if (od == null) break;
                        order2 += od + ",";
                    }
                    order2 = order2.Remove(order2.Length - 1);
                }
                prefinTable.Add(new Preference()
                {
                    Date = DateTime.Parse(dates[index]),
                    //FavoriteColor = int.Parse(favcolor[index]),
                    Id = Guid.NewGuid(),
                    Preference1 = "Золотая",
                    Oder1 = order1,
                    Oder2 = order2,
                    ShortOder1 = shorder1,
                    ShortOder2 = shorder2,
                    Preference2 = pref2[index] ?? "null",
                    Compare = pref1[index] == (pref2[index] ?? "") ? "true" : "false",
                    UserId = $"Ex21#{number[index]}"
                });
            }

            foreach (var pref in prefinTable)
            {
                dataRepository.Preference.InsertOnSubmit(pref);
                dataRepository.SubmitChanges();
            }
            #endregion

            foreach (object s in (Array)excelRange)
            {
                Console.WriteLine(s);
            }

            for (int i = 2; i <= excelRange.Count + 1; i++)
            {
                string values = sh.Cells[i, 2].ToString();
            }
        }
        #endregion
    }
}