namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Enums;
    using VipFit.Core.Models;

    public class PassTemplateViewModel : ObservableRecipient
    {
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
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model has been modified.
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show a progress bar.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is new PassTemplate.
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
                if (value == Model.Type)
                    return;

                Model.Type = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));
                OnPropertyChanged(nameof(MonthsDuration));
                OnPropertyChanged(nameof(Entries));
            }
        }

        /// <summary>
        /// Gets or sets the PassTemplate duration.
        /// </summary>
        public PassDuration Duration
        {
            get => Model.Duration;
            set
            {
                if (value == Model.Duration)
                    return;

                Model.Duration = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));
                OnPropertyChanged(nameof(MonthsDuration));
                OnPropertyChanged(nameof(Entries));
            }
        }

        /// <summary>
        /// Gets or sets the PassTemplate price.
        /// </summary>
        public decimal Price
        {
            get => Model.Price;
            set
            {
                if (value == Model.Price)
                    return;

                Model.Price = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));
            }
        }

        /// <summary>
        /// Gets the PassTemplate duration in months.
        /// </summary>
        public byte MonthsDuration => Model.MonthsDuration;

        /// <summary>
        /// Gets the PassTemplate total entries to the gym.
        /// </summary>
        public byte Entries => Model.Entries;

        /// <summary>
        /// Gets the PassCode.
        /// </summary>
        public string PassCode => Model.PassCode;

        /// <summary>
        /// Gets collection of available pass types.
        /// </summary>
        public List<string> AvailablePassTypes => Enum.GetNames(typeof(PassType)).ToList();

        /// <summary>
        /// Gets collection of available pass durations.
        /// </summary>
        public List<string> AvailablePassDurations => Enum.GetNames(typeof(PassDuration)).ToList();

        #endregion

        public void SetType(object value)
        {
            if (value == null)
                return;

            Type = (PassType)Enum.Parse(typeof(PassType), (string)value);
        }

        public void SetDuration(object value)
        {
            if (value == null)
                return;

            Duration = (PassDuration)Enum.Parse(typeof(PassDuration), (string)value);
        }

        public void SetPrice(double value) => Price = Convert.ToDecimal(value);

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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
        /// Reloads all of the PassTemplate data.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshPassTemplateAsync() => Model = await App.GetService<IPassTemplateRepository>().GetAsync(Model.Id);
        
        internal static byte GetMonths(PassType type, PassDuration duration) => duration switch
        {
            PassDuration.Short => (byte)(type == PassType.Standard ? 3 : 1),
            PassDuration.Medium => (byte)(type == PassType.Standard ? 6 : 3),
            PassDuration.Long => (byte)(type == PassType.Standard ? 12 : 10),
            _ => throw new NotImplementedException(),
        };

        internal static byte GetTotalEntries(byte months, PassType type) => (byte)(months * (byte)type);
    }
}