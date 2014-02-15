using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Test1.Helper;
using Test1.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;

namespace Test1.ViewModel
{
    class MaterialViewModel : BaseViewModel
    {
        #region Properties

        private string xmlPath;

        /// <summary>
        /// Source path of XML file
        /// </summary>
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

        /// <summary>
        /// Application top Error message.
        /// </summary>
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

        /// <summary>
        /// Collection of all materials in XML. Populated from given XML file
        /// </summary>
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

        // Lock object to prevent concurrency issue
        private object lockObject = new object();

        #endregion

        #region Command

        private ICommand xmlProcessCommand;

        /// <summary>
        /// Relay Command to process a XML file, load materials in application
        /// </summary>
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

        /// <summary>
        /// Relay Command to search a given material name in Material collection
        /// </summary>
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
        }

        #endregion

        #region Actions

        /// <summary>
        /// Delegate to Process XML, and load all materials in collection
        /// </summary>
        /// <param name="param">XML File source path</param>
        public void ProcessXML(object param)
        {
            string fileName = param as string;

            // Clear Error Message in case we are processing different XML files on same instance
            this.ErrorMessage = string.Empty;

            lock (lockObject)
            {
                XDocument xmlDoc = null;
                try
                {
                    // Read XML using Linq
                    xmlDoc = XMLParser.GetXMLDataFromFileName(fileName);
                }
                catch (FileNotFoundException ex)
                {
                    this.ErrorMessage = ex.Message;
                }

                if (xmlDoc != null)
                {
                    // Fetch Data from XML using Linq - Hard coded element names. Can also use Descendants
                    var materials = (from material in xmlDoc.Element("acousticmaterial").Element("mtllib").Elements("material")
                                     select new
                                     {
                                         Name = (string)material.Attribute("name"),
                                         Absorption = (string)material.Attribute("absorption"),
                                         Scattering = (string)material.Attribute("scattering")
                                     });


                    this.MaterialListCollection.IsUpdatePaused = true;

                    // Add all material to collection
                    foreach (var material in materials)
                    {
                        this.MaterialListCollection.Add(new Material { Name = material.Name, Absorption = material.Absorption, Scattering = material.Scattering });
                    }

                    this.MaterialListCollection.IsUpdatePaused = false;
                }
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
