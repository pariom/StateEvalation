﻿using StateEvaluation.Model;
using StateEvaluation.ViewModel.PreferenceDataGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using StateEvaluation.Enums;

namespace StateEvaluation.Providers
{
    public class PreferenceDataGridProvider
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();
        private List<string> _color1in3s = new List<string>();
        private List<string> _color2in3s = new List<string>();
        private List<string> _color1in12s = new List<string>();
        private List<string> _color2in12s = new List<string>();
        private List<bool> _preference1 = new List<bool>();
        private List<bool> _preference2 = new List<bool>();

        public string SavePreference(PreferenceDto preferenceDto)
        {
            try
            {
                var preference = GetNewPreference(preferenceDto);

                _preferenceDb.InsertPreference(preference);
                ClearInputs(preferenceDto);

                return preference.Id.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool IsValidPreferenseDto(PreferenceDto preferenceDto)
        {
            _color1in3s = new List<string>() { preferenceDto.Color1in3, preferenceDto.Color2in3, preferenceDto.Color3in3 };

            _color2in3s = new List<string>() { preferenceDto.Color21in3, preferenceDto.Color22in3, preferenceDto.Color23in3 };

            _color1in12s = new List<string>() {
                preferenceDto.Color1in12, preferenceDto.Color2in12, preferenceDto.Color3in12,
                preferenceDto.Color4in12, preferenceDto.Color5in12, preferenceDto.Color6in12,
                preferenceDto.Color7in12, preferenceDto.Color8in12, preferenceDto.Color9in12,
                preferenceDto.Color10in12, preferenceDto.Color11in12, preferenceDto.Color12in12
            };

            _color2in12s = new List<string>() {
                preferenceDto.Color21in12, preferenceDto.Color22in12, preferenceDto.Color23in12,
                preferenceDto.Color24in12,  preferenceDto.Color25in12, preferenceDto.Color26in12,
                preferenceDto.Color27in12, preferenceDto.Color28in12,  preferenceDto.Color29in12,
                preferenceDto.Color210in12,  preferenceDto.Color211in12, preferenceDto.Color212in12
            };

            _preference1 = new List<bool>() { bool.Parse(preferenceDto.Preference1Blue ?? StringBooleanValues.False), bool.Parse(preferenceDto.Preference1Grey ?? StringBooleanValues.False),
                bool.Parse(preferenceDto.Preference1Yellow ?? StringBooleanValues.False), bool.Parse(preferenceDto.Preference1Red ?? StringBooleanValues.False) };
            _preference2 = new List<bool>() { bool.Parse(preferenceDto.Preference2Blue ?? StringBooleanValues.False), bool.Parse(preferenceDto.Preference2Grey ?? StringBooleanValues.False),
                bool.Parse(preferenceDto.Preference2Yellow ?? StringBooleanValues.False), bool.Parse(preferenceDto.Preference2Red ?? StringBooleanValues.False) };

            var userId = new List<string> { preferenceDto.UserId };

            if (false && (new List<List<string>> { userId, _color1in3s, _color2in3s, _color1in12s, _color2in12s }.Any(x => x.Any(y => y == null) || x.Count != x.Distinct().Count())
                || new List<List<bool>> { _preference1, _preference2 }.Any(x => x.All(y => y == false))
                || preferenceDto.TestDate == null))
            {
                return BooleanValues.False;
            }
            return BooleanValues.True;
        }

        private Preference GetNewPreference(PreferenceDto preferenceDto)
        {
            List<byte> Color1in3sByte = _color1in3s.Select(x => Byte.Parse(x)).ToList();
            List<byte> Color1in12sByte = _color1in12s.Select(x => Byte.Parse(x)).ToList();
            List<byte> Color2in3sByte = _color2in3s.Select(x => Byte.Parse(x)).ToList();
            List<byte> Color2in12sByte = _color2in12s.Select(x => Byte.Parse(x)).ToList();
            
            var Preference1Index = _preference1.IndexOf(BooleanValues.True);
            var Preference2Index = _preference2.IndexOf(BooleanValues.True);
            Preference preference = new Preference()
            {
                //Id = Guid.NewGuid(),
                UserId = preferenceDto.UserId,
                Date = DateTime.Parse(preferenceDto.TestDate),
                FavoriteColor = 0,
                ShortOder1 = String.Join(",", _color1in3s),
                Oder1 = String.Join(",", _color1in12s),
                Preference1 = new StateEvaluationDLL.DataStructures.Preference(Color1in3sByte, Color2in3sByte).Type.ToString(), 
                ShortOder2 = String.Join(",", _color2in3s),
                Oder2 = String.Join(",", _color2in12s),
                Preference2 = new StateEvaluationDLL.DataStructures.Preference(Color2in3sByte, Color2in12sByte).Type.ToString(),
                Compare = (_preference1.IndexOf(BooleanValues.True) == _preference2.IndexOf(BooleanValues.True)).ToString().ToLower(),
                RelaxTable1 = int.Parse(preferenceDto.ColorRelax1),
                RelaxTable2 = int.Parse(preferenceDto.ColorRelax2)
            };

            return preference;
        }

        private void ClearInputs(PreferenceDto preferenceDto)
        {
            foreach(var i in preferenceDto.GetType().GetProperties())
            {
                i.SetValue(preferenceDto, string.Empty);
            }
        }

        public IEnumerable<Preference> GetAllPreferences()
        {
            return _preferenceDb.GetAllTests();
        }
    }
}
