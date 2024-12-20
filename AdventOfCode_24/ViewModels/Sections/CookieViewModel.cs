﻿using AdventOfCode_24.Model.WebConnection;

namespace AdventOfCode_24.ViewModels.Sections
{
    public class CookieViewModel : ViewModelBase
    {
        private string _cookie;

        public string Cookie
        {
            get => _cookie;
            set
            {
                _cookie = value;
                OnPropertyChanged(nameof(Cookie));
            }
        }

        public CookieViewModel()
        {
            _cookie = CookieData.ActiveCookie;
        }

        public void SaveCookie()
        {
            CookieData.SetCookie(Cookie);
        }
    }
}
