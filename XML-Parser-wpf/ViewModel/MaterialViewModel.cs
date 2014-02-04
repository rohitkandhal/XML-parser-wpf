using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Test1.Helper;
using Test1.Model;
using System.Collections.ObjectModel;

namespace Test1.ViewModel
{
    class MaterialViewModel : BaseViewModel
    {
        #region Properties

        private string xmlPath;

        public string XMLPath
        {
            get
            {
                return xmlPath;
            }
            set
            {
                this.xmlPath = value;
                this.OnPropertyChanged("XMLPath");
            }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        private string searchQuery;

        public string SearchQuery
        {
            get
            {
                return this.searchQuery;
            }
            set
            {
                this.searchQuery = value;
                this.OnPropertyChanged("SearchQuery");
            }
        }

        private MyObservableCollection<Material> materialListCollection;

        public MyObservableCollection<Material> MaterialListCollection
        {
            get
            {
                if (materialListCollection == null)
                {
                    this.materialListCollection = new MyObservableCollection<Material>();
                }
                return materialListCollection;
            }
            set
            {
                materialListCollection = value;
                this.OnPropertyChanged("MaterialList");
            }
        }

        private ObservableCollection<Material> materialSearchResults;

        public ObservableCollection<Material> MaterialSearchResults
        {
            get
            {
                if (materialSearchResults == null)
                {
                    this.materialSearchResults = new ObservableCollection<Material>();
                }
                return materialSearchResults;
            }
            set
            {
                materialSearchResults = value;
                this.OnPropertyChanged("MaterialSearchResults");
            }

        }

        private object lockObject = new object();

        #endregion

        #region Command

        private ICommand xmlProcessCommand;

        public ICommand XMLProcessCommand
        {
            get
            {
                if (xmlProcessCommand == null)
                {
                    xmlProcessCommand = new RelayCommand(ProcessXML);
                }
                return xmlProcessCommand;
            }
        }

        private ICommand searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(MaterialSearchByName);
                }
                return searchCommand;
            }
        }

        #endregion

        #region Constructor

        public MaterialViewModel()
        {
            this.ErrorMessage = string.Empty;
        }

        #endregion
        #region Actions

        /// <summary>
        /// Delegate to Process XML
        /// </summary>
        /// <param name="param"></param>
        public void ProcessXML(object param)
        {
            string fileName = param as string;

            // Validate File Path
            if (fileName == null || !System.IO.File.Exists((string)fileName))
            {
                this.ErrorMessage = "Error: File does not exist or invalid file";
                return;
            }
            else
            {
                this.ErrorMessage = string.Empty;
            }

            lock (lockObject)
            {
                // Read XML using Linq
                var doc = XMLParser.GetXMLDataFromFileName(fileName);

                // Using Hard Coded path. Can also use Descendants
                var materials = (from material in doc.Element("acousticmaterial").Element("mtllib").Elements("material")
                                 select new
                                 {
                                     Name = (string)material.Attribute("name"),
                                     Absorption = (string)material.Attribute("absorption"),
                                     Scattering = (string)material.Attribute("scattering")
                                 });

                // Populate all material to collection
                this.MaterialListCollection.IsUpdatePaused = true;

                foreach (var material in materials)
                {
                    this.MaterialListCollection.Add(new Material { Name = material.Name, Absorption = material.Absorption, Scattering = material.Scattering });
                }

                this.MaterialListCollection.IsUpdatePaused = false;
            }
        }

        public void MaterialSearchByName(object param)
        {
            string searchQuery = param as string;

            if (searchQuery == null)
            {
                this.ErrorMessage = "Error: search query invalid";
                return;
            }

            this.MaterialSearchResults.Clear();

            this.MaterialSearchResults = new ObservableCollection<Material>(this.MaterialListCollection.Where(x => x.Name.Contains(searchQuery)));

        }
        #endregion
    }
}
