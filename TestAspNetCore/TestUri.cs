using System;

namespace TestNetCore
{
    public static class TestUri
    {
        public static void Run()
        {
            var relativeUri = new Uri(
                "srv/billy/Billy.Test40_settings-backup_2021-05-06_11-13-03.zip",
                UriKind.Relative);
            Console.WriteLine(string.Join(Environment.NewLine, relativeUri.Segments));
        }
    }
}
