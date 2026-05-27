namespace PlantifyAdmin.Services
{
    public class ToastMessage
    {
        public string Message { get; set; } = "";
        public string Type { get; set; } = "success"; // success, error, warning, info
        public string Icon { get; set; } = "✅";
    }

    public class ToastService
    {
        public event Action<ToastMessage>? OnShow;

        public void Show(string message, string type = "success")
        {
            var icon = type switch
            {
                "success" => "✅",
                "error" => "❌",
                "warning" => "⚠️",
                "info" => "ℹ️",
                _ => "✅"
            };

            OnShow?.Invoke(new ToastMessage { Message = message, Type = type, Icon = icon });
        }
    }
}