namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.UI.Dispatching;
    using Microsoft.UI.Xaml;
    using VipFit.Core.Enums;
    using VipFit.Core.Models;
    using VipFit.DataAccessLayer;

    /// <summary>
    /// Client VM.
    /// </summary>
    public class ClientViewModel : ObservableRecipient
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private Client model;

        private bool isLoading;
        private bool isNewClient;
        private bool isInEdit = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientViewModel"/> class.
        /// </summary>
        /// <param name="model">Client view model.</param>
        public ClientViewModel(Client? model = null) => Model = model ?? new Client();

        /// <summary>
        /// Raised when the user cancels the changes they've made to the client data.
        /// </summary>
        public event EventHandler AddNewClientCanceled;

        /// <summary>
        /// Gets or sets the underlying Customer object.
        /// </summary>
        public Client Model
        {
            get => model;
            set
            {
                if (model != value)
                {
                    model = value;

                    // Raise the PropertyChanged event for all properties
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the underlying model has been modified. 
        /// </summary>
        /// <remarks>
        /// Used when syncing with the server to reduce load and only upload the models that have changed.
        /// </remarks>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to show a progress bar. 
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this is new client.
        /// </summary>
        public bool IsNewClient
        {
            get => isNewClient;
            set => SetProperty(ref isNewClient, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the client data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => isInEdit;
            set => SetProperty(ref isInEdit, value);
        }

        #region Model's Properties

        /// <summary>
        /// Gets or sets the client's first name.
        /// </summary>
        public string FirstName
        {
            get => Model.FirstName;
            set
            {
                if (value != Model.FirstName)
                {
                    Model.FirstName = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Gets or sets the client's last name.
        /// </summary>
        public string LastName
        {
            get => Model.LastName;
            set
            {
                if (value != Model.LastName)
                {
                    Model.LastName = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Gets the client's full (first + last) name.
        /// </summary>
        public string Name =>
            IsNewClient &&
            string.IsNullOrEmpty(FirstName) &&
            string.IsNullOrEmpty(LastName) ?
            "New Client" : $"{FirstName} {LastName}";

        /// <summary>
        /// Gets or sets the client's phone number.
        /// </summary>
        public string Phone
        {
            get => Model.Phone;
            set
            {
                if (value != Model.Phone)
                {
                    Model.Phone = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the client's email.
        /// </summary>
        public string Email
        {
            get => Model.Email;
            set
            {
                if (value != Model.Email)
                {
                    Model.Email = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        #region Agreements

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for all purposes.
        /// </summary>
        public bool? AgreementsAll
        {
            get
            {
                if (AgreementMarketing && AgreementPromoImage && AgreementWebsiteImage && AgreementSocialsImage)
                    return true;
                if (AgreementMarketing || AgreementPromoImage || AgreementWebsiteImage || AgreementSocialsImage)
                    return null;
                if (!AgreementMarketing && !AgreementPromoImage && !AgreementWebsiteImage && !AgreementSocialsImage)
                    return false;
                else return null;
            }

            set
            {
                if (value == null) { return; }
                AgreementMarketing = (bool)value;
                OnPropertyChanged(nameof(AgreementMarketing));
                AgreementPromoImage = (bool)value;
                OnPropertyChanged(nameof(AgreementPromoImage));
                AgreementWebsiteImage = (bool)value;
                OnPropertyChanged(nameof(AgreementWebsiteImage));
                AgreementSocialsImage = (bool)value;
                OnPropertyChanged(nameof(AgreementSocialsImage));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for marketing purposes.
        /// </summary>
        public bool AgreementMarketing
        {
            get => Model.AgreementMarketing;
            set
            {
                if (value != Model.AgreementMarketing)
                {
                    Model.AgreementMarketing = value;
                    IsModified = true;
                    model.ModifiedAt = DateTime.Now;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AgreementsAll));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for using image for promotional purposes.
        /// </summary>
        public bool AgreementPromoImage
        {
            get => Model.AgreementPromoImage;
            set
            {
                if (value != Model.AgreementPromoImage)
                {
                    Model.AgreementPromoImage = value;
                    IsModified = true;
                    model.ModifiedAt = DateTime.Now;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AgreementsAll));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for using image on website.
        /// </summary>
        public bool AgreementWebsiteImage
        {
            get => Model.AgreementWebsiteImage;
            set
            {
                if (value != Model.AgreementWebsiteImage)
                {
                    Model.AgreementWebsiteImage = value;
                    IsModified = true;
                    model.ModifiedAt = DateTime.Now;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AgreementsAll));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether user agreed for using image on socials.
        /// </summary>
        public bool AgreementSocialsImage
        {
            get => Model.AgreementSocialsImage;
            set
            {
                if (value != Model.AgreementSocialsImage)
                {
                    Model.AgreementSocialsImage = value;
                    IsModified = true;
                    model.ModifiedAt = DateTime.Now;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AgreementsAll));
                }
            }
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// Gets or sets comment for client.
        /// </summary>
        public string Comment
        {
            get => Model.Comment;
            set
            {
                if (value != Model.Comment)
                {
                    Model.Comment = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets client's status.
        /// </summary>
        public ClientStatus Status
        {
            get => Model.Status;
            set
            {
                if (value != Model.Status)
                {
                    Model.Status = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether client is removed and is in "bin".
        /// </summary>
        public bool Trash
        {
            get => Model.Trash;
            set
            {
                if (value != Model.Trash)
                {
                    Model.Trash = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #endregion

        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;

            if (IsNewClient)
            {
                IsNewClient = false;
                Model.CreatedAt = DateTime.Now;
                App.GetService<ClientListViewModel>().Clients.Add(this);
            }

            Model.ModifiedAt = DateTime.Now;
            await App.GetService<IClientRepository>().UpsertAsync(Model);
        }

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNewClient)
                AddNewClientCanceled?.Invoke(this, EventArgs.Empty);
            else
                await RevertChangesAsync();
        }

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            if (Model != null)
            {
                IsModified = false;
                App.GetService<ClientListViewModel>().Clients.Remove(this);
                await App.GetService<IClientRepository>().DeleteAsync(Model.Id);
            }
        }

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        public async Task RevertChangesAsync()
        {
            IsInEdit = false;
            if (IsModified)
            {
                await RefreshClientAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Called when a bound DataGrid control causes the client to enter edit mode.
        /// </summary>
        public void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Reloads all of the client data.
        /// </summary>
        public async Task RefreshClientAsync()
        {
            Model = await App.GetService<IClientRepository>().GetAsync(Model.Id);
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a client.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a client.
        /// </summary>
        public async void EndEdit() => await SaveAsync();

    }
}