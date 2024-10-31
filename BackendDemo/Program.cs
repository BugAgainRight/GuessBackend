using BackendDemo;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.SelfHost;

Storage.LoadFromFile();

// 这个类的东西都不用改，放着不动就好
var config = new HttpSelfHostConfiguration("http://localhost:8633");
config.EnableCors();
config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

config.Routes.MapHttpRoute(  
    name: "DefaultApi", 
    routeTemplate: "api/{controller}/{action}",  
    defaults: new { id = RouteParameter.Optional }  
);

var server = new HttpSelfHostServer(config);
server.OpenAsync().Wait();  

Console.WriteLine("服务器已启动，按回车停止。");
Console.ReadLine();