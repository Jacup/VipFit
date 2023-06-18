namespace VipFit
{
    /// <summary>
    /// Represents Main window.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
            Content = null;
        }
    }
}