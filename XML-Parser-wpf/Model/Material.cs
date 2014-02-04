using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Test1.Model
{
    class Material : INotifyPropertyChanged
    {
        #region Properties

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string absorption;

        public string Absorption
        {
            get
            {
                return absorption;
            }
            set
            {
                absorption = value;
                OnPropertyChanged("Absorption");
            }
        }

        private string scattering;

        public string Scattering
        {
            get
            {
                return scattering;
            }
            set
            {
                scattering = value;
                OnPropertyChanged("Scattering");
            }
        }

        #endregion

        #region Constructor

        public Material()
        {
            this.Name = string.Empty;
            this.Absorption = string.Empty;
            this.Scattering = string.Empty;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
