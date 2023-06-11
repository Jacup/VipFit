namespace VipFit.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.UI.Dispatching;

    public abstract class BaseViewModel : ObservableRecipient
    {
        private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        private bool isLoading;
        private bool isNew;
        private bool isInEdit = false;

        /// <summary>
        /// Gets or sets a value indicating whether the underlying model has been modified.
        /// </summary>
        /// <remarks>
        /// Used when syncing with the server to reduce load and only upload the models that have changed.
        /// </remarks>
        public bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show a progress bar.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is new object.
        /// </summary>
        public bool IsNew
        {
            get => isNew;
            set => SetProperty(ref isNew, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the object data is being edited.
        /// </summary>
        public bool IsInEdit
        {
            get => isInEdit;
            set => SetProperty(ref isInEdit, value);
        }

        protected DispatcherQueue DispatcherQueue => dispatcherQueue;

        /// <summary>
        /// Save changes to database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task SaveAsync();

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task DeleteAsync();

        /// <summary>
        /// Cancels any in progress edits.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task CancelEditsAsync();

        /// <summary>
        /// Discards any edits that have been made, restoring the original values.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task RevertChangesAsync();
    }
}
