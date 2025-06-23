namespace JobRecorderNet.Models
{
    public class SearchBarViewModel
    {
        public required string Title { get; set; }
        public string? Search { get; set; }
        public string? PlaceHolder { get; set; }
        // Controller routes
        public required string IndexRoute { get; set; }
        public required string CreateRoute { get; set; }
        public Dictionary<string, string>? Columns { get; set; }
        public string? SelectedColumn { get; set; }

    }
}