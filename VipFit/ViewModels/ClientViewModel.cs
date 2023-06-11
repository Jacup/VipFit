namespace VipFit.ViewModels
{
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Dispatching;
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;
    using Windows.ApplicationModel.Resources;

    /// <summary>
    /// Client VM.
    /// </summary>
    public class ClientViewModel : BaseViewModel
    {
        private Client model;
        private Pass? selectedPass;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientViewModel"/> class.
        /// </summary>
        /// <param name="model">Client view model.</param>
        public ClientViewModel(Client? model = null)
        {
            Model = model ?? new Client();
        }

        /// <summary>
        /// Raised when the user cancels the changes they've made to the client data.
        /// </summary>
        public event EventHandler AddNewClientCanceled;

        /// <summary>
        /// Gets or sets the underlying Client object.
        /// </summary>
        public Client Model
        {
            get => model;
            set
            {
                if (model == value)
                    return;

                model = value;
                RefreshPasses();

                OnPropertyChanged(string.Empty);
            }
        }

        /// <summary>
        /// Gets collection of passes.
        /// </summary>
        public ObservableCollection<Pass> Passes { get; } = new();

        /// <summary>
        /// Gets collection of entries.
        /// </summary>
        public ObservableCollection<Entry> Entries { get; } = new();

        /// <summary>
        /// Gets or sets the selected pass.
        /// </summary>
        public Pass? SelectedPass
        {
            get => selectedPass;
            set
            {
                SetProperty(ref selectedPass, value);
                RefreshEntries();
            }
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
            IsNew &&
            string.IsNullOrEmpty(FirstName) &&
            string.IsNullOrEmpty(LastName) ?
            ResourceLoader.GetForViewIndependentUse("Resources").GetString("NewClient") : $"{FirstName} {LastName}";

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
                if (value == null)
                    return;
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

        /// <inheritdoc/>
        public override async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;

            if (IsNew)
            {
                IsNew = false;
                Model.CreatedAt = DateTime.Now;
                App.GetService<ClientListViewModel>().Clients.Add(this);
            }

            Model.ModifiedAt = DateTime.Now;
            await App.GetService<IClientRepository>().UpsertAsync(Model);
        }

        /// <inheritdoc/>
        public override async Task CancelEditsAsync()
        {
            if (IsNew)
                AddNewClientCanceled?.Invoke(this, EventArgs.Empty);
            else
                await RevertChangesAsync();
        }

        /// <inheritdoc/>
        public override async Task DeleteAsync()
        {
            if (Model != null)
            {
                IsModified = false;
                App.GetService<ClientListViewModel>().Clients.Remove(this);
                await App.GetService<IClientRepository>().DeleteAsync(Model.Id);
            }
        }

        /// <inheritdoc/>
        public override async Task RevertChangesAsync()
        {
            IsInEdit = false;

            if (IsNew)
            {
                IsModified = false;
                return;
            }

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
        /// Reloads all of the client data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshClientAsync()
        {
            Model = await App.GetService<IClientRepository>().GetAsync(Model.Id);

            var newPasses = await App.GetService<IPassRepository>().GetForClientAsync(Model.Id);

            if (!newPasses.Any())
                return;

            Passes.Clear();
            foreach (var p in newPasses)
                Passes.Add(p);
        }

        /// <summary>
        /// Resets the customer detail fields to the current values.
        /// </summary>
        public async void RefreshPasses()
        {
            await LoadPassesAsync();
        }

        /// <summary>
        /// Resets the customer detail fields to the current values.
        /// </summary>
        public async void RefreshEntries()
        {
            await LoadEntriesAsync();
        }

        /// <summary>
        /// Loads the entries for client.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoadPassesAsync()
        {
            await DispatcherQueue.EnqueueAsync(() =>
            {
                IsLoading = true;
            });

            var passes = await App.GetService<IPassRepository>().GetForClientAsync(Model.Id);

            await DispatcherQueue.EnqueueAsync(() =>
            {
                Passes.Clear();
                foreach (var p in passes)
                    Passes.Add(p);

                IsLoading = false;
            });
        }

        /// <summary>
        /// Loads the client's entries.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoadEntriesAsync()
        {
            await DispatcherQueue.EnqueueAsync(() =>
            {
                IsLoading = true;
            });

            if (SelectedPass == null)
                return;

            var entries = await App.GetService<IEntryRepository>().GetForPassAsync(SelectedPass.Id);

            await DispatcherQueue.EnqueueAsync(() =>
            {
                Entries.Clear();
                foreach (var e in entries)
                    Entries.Add(e);

                IsLoading = false;
            });
        }
    }
}