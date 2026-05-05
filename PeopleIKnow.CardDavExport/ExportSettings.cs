namespace PeopleIKnow.CardDavExport
{
    public class ExportSettings
    {
        public string ServerUrl { get; set; }
        public string AddressBookPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; } = "Data Source=../PeopleIKnow/people.db";
        public string WebRootPath { get; set; } = "../PeopleIKnow/wwwroot";
        public string ExportMode { get; set; } = "file";
        public string OutputDirectory { get; set; } = "./export";
    }
}
