namespace VipFit.ViewModels
{
    using System.Collections.ObjectModel;
    using VipFit.Core.DataAccessLayer.Interfaces;
    using VipFit.Core.Models;

    /// <summary>
    /// Pass Template VM.
    /// </summary>
    public class PassTemplateViewModel : BaseViewModel
    {
        private PassTemplate model;

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

        #region Model's Properties

        /// <summary>
        /// Gets or sets the PassTemplate name.
        /// </summary>
        public string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name == value)
                    return;

                Model.Name = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));
            }
        }

        /// <summary>
        /// Gets collection of available pass durations.
        /// </summary>
        public ObservableCollection<string> SuggestedNames { get; } = new() { "Standard", "Pro", "Open" };

        /// <summary>
        /// Gets or sets the PassTemplate duration in months.
        /// </summary>
        public byte MonthsDuration
        {
            get => Model.MonthsDuration;
            set
            {
                if (Model.MonthsDuration == value)
                    return;

                Model.MonthsDuration = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalEntries));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(IsModified));
            }
        }

        /// <summary>
        /// Gets or sets the PassTemplate price.
        /// </summary>
        public decimal PricePerMonth
        {
            get => Model.PricePerMonth;
            set
            {
                if (Model.PricePerMonth == value)
                    return;

                Model.PricePerMonth = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(IsModified));
            }
        }

        /// <summary>
        /// Gets or sets the amount of entries to the gym per month.
        /// </summary>
        public byte EntriesPerMonth
        {
            get => Model.EntriesPerMonth;
            set
            {
                if (Model.EntriesPerMonth == value)
                    return;

                Model.EntriesPerMonth = value;
                IsModified = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalEntries));
                OnPropertyChanged(nameof(IsModified));
            }
        }

        /// <summary>
        /// Gets the PassTemplate total entries to the gym.
        /// </summary>
        public int TotalEntries => Model.TotalEntries;

        /// <summary>
        /// Gets the PassTemplate total entries to the gym.
        /// </summary>
        public decimal TotalPrice => Model.TotalPrice;

        /// <summary>
        /// Gets the PassCode.
        /// </summary>
        public string PassCode => Model.PassCode;

        #endregion

        /// <inheritdoc/>
        public override async Task SaveAsync()
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

        /// <inheritdoc/>
        public override async Task CancelEditsAsync()
        {
            if (IsNew)
                AddNewPassTemplateCanceled?.Invoke(this, EventArgs.Empty);
            else
                await RevertChangesAsync();
        }

        /// <inheritdoc/>
        public override async Task DeleteAsync()
        {
            if (Model != null)
            {
                IsModified = false;
                App.GetService<PassTemplateListViewModel>().PassTemplates.Remove(this);
                await App.GetService<IPassTemplateRepository>().DeleteAsync(Model.Id);
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
        public async Task RefreshPassTemplateAsync()
        {
            Model = await App.GetService<IPassTemplateRepository>().GetAsync(Model.Id);
        }
    }
}