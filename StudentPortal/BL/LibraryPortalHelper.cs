using Newtonsoft.Json;
using StudentPortal.Interface;
using System.Dynamic;

namespace StudentPortal.BL
{
    public class LibraryPortalHelper : ILibraryPortalHelper
    {
        private readonly IConfigurationRoot configuration;
        private readonly string API_URL = string.Empty;

        public LibraryPortalHelper()
        {
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            //Read API url from config file
            API_URL = $"{configuration.GetValue<string>("LibraryAppUrl")}api/";
        }

        public string RegisterUser(string userId, string email)
        {
            var url = $"{API_URL}Account/Register";

            var apiHelper = new APIHelper();

            dynamic obj = new ExpandoObject();

            obj.StudentId = userId;
            obj.Email = email;

            var response = apiHelper.PostMethod(url, obj);

            var result = JsonConvert.DeserializeObject<string>(response);
            return result;
        }
    }
}
