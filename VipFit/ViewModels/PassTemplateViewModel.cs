namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.UI.Dispatching;
    using VipFit.Core.DataAccessLayer;
    using VipFit.Core.Enums;
    using VipFit.Core.Models;

    public class PassTemplateViewModel : ObservableRecipient
    {
        private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private PassTemplate model;

        private bool isLoading;
        private bool isNew;
        private bool isInEdit = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassTemplateViewModel"/> class.
        /// </summary>
        /// <param name="model">PassTemplate view model.</param>
        public PassTemplateViewModel(PassTemplate? model = null) => Model = model ?? new PassTemplate();

        /// <summary>
        /// Raised when the user cancels the changes they've made to the PassTemplate data.
        /// </summary>
        public event EventHandler AddNewPassTemplateCanceled;

        /// <summary>
        /// Gets or sets the underlying Customer object.
        /// </summary>
        public PassTemplate Model
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
        /// Gets or sets a value that indicates whether this is new PassTemplate.
        /// </summary>
        public bool IsNew
        {
            get => isNew;
            set => SetProperty(ref isNew, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the PassTemplate data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => isInEdit;
            set => SetProperty(ref isInEdit, value);
        }

        #region Model's Properties

        /// <summary>
        /// Gets or sets the PassTemplate type.
        /// </summary>
        public PassType Type
        {
            get => Model.Type;
            set
            {
                if (value != Model.Type)
                {
                    Model.Type = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsModified));
                    OnPropertyChanged(nameof(MonthsDuration));
                    OnPropertyChanged(nameof(Entries));
                }
            }
        }

        public List<string> AvailablePassTypes = Enum.GetNames(typeof(PassType)).ToList();

        public void SetType(object value)
        {
            if (value == null)
                return;

            Type = (PassType)Enum.Parse(typeof(PassType), value as string);
        }

        /// <summary>
        /// Gets or sets the PassTemplate duration.
        /// </summary>
        public PassDuration Duration
        {
            get => Model.Duration;
            set
            {
                if (value != Model.Duration)
                {
                    Model.Duration = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsModified));
                    OnPropertyChanged(nameof(MonthsDuration));
                    OnPropertyChanged(nameof(Entries));
                }
            }
        }

        public List<string> AvailablePassDurations = Enum.GetNames(typeof(PassDuration)).ToList();

        public void SetDuration(object value)
        {
            if (value == null)
                return;

            Duration = (PassDuration)Enum.Parse(typeof(PassDuration), value as string);
        }

        /// <summary>
        /// Gets or sets the PassTemplate price.
        /// </summary>
        public decimal Price
        {
            get => Model.Price;
            set
            {
                if (value != Model.Price)
                {
                    Model.Price = value;
                    IsModified = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsModified));
                }
            }
        }

        public void SetPrice(double value) => Price = Convert.ToDecimal(value);


        /// <summary>
        /// Gets the PassTemplate duration in months.
        /// </summary>
        public byte MonthsDuration => Model.MonthsDuration;

        /// <summary>
        /// Gets the PassTemplate total entries to the gym.
        /// </summary>
        public byte Entries => Model.Entries;

        /// <summary>
        /// Gets the PassTemplate total entries to the gym.
        /// </summary>
        public string PassCode => Model.PassCode;

        #endregion

        public async Task SaveAsync()
        {
            IsInEdit = false;
            IsModified = false;

            if (IsNew)
            {
                IsNew = false;
                App.GetService<PassTemplateListViewModel>().PassTemplates.Add(this);
            }

            await App.GetService<IPassTemplateRepository>().UpsertAsync(Model);
        }

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        public async Task CancelEditsAsync()
        {
            if (IsNew)
                AddNewPassTemplateCanceled?.Invoke(this, EventArgs.Empty);
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
                App.GetService<PassTemplateListViewModel>().PassTemplates.Remove(this);
                await App.GetService<IPassTemplateRepository>().DeleteAsync(Model.Id);
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
                await RefreshPassTemplateAsync();
                IsModified = false;
            }
        }

        /// <summary>
        /// Enables edit mode.
        /// </summary>
        public void StartEdit() => IsInEdit = true;

        /// <summary>
        /// Called when a bound DataGrid control causes the PassTemplate to enter edit mode.
        /// </summary>
        public void BeginEdit()
        {
            // Not used.
        }

        /// <summary>
        /// Reloads all of the PassTemplate data.
        /// </summary>
        public async Task RefreshPassTemplateAsync()
        {
            Model = await App.GetService<IPassTemplateRepository>().GetAsync(Model.Id);
        }

        /// <summary>
        /// Called when a bound DataGrid control cancels the edits that have been made to a PassTemplate.
        /// </summary>
        public async void CancelEdit() => await CancelEditsAsync();

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a PassTemplate.
        /// </summary>
        public async void EndEdit() => await SaveAsync();

    }
}