using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace ApiNba.Helpers
{
    public enum Folders { Equipos = 0, Jugadores = 1, Partidos = 2, Fondos = 3 }
    public class HelperPathProvider
    {
        //NECESITAMOS ACCEDER AL SISTEMA DE ARCHIVOS DEL WEB SERVER (wwwroot) 
        private IServer server;
        public HelperPathProvider(IServer server)
        {
            this.server = server;
        }

        public string GetFolderPath( Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Equipos)
            {
                carpeta = "equipos";
            }
            else if (folder == Folders.Jugadores)
            {
                carpeta = "jugadores";
            }
            else if (folder == Folders.Partidos)
            {
                carpeta = "partidos";
            }
            else if (folder == Folders.Fondos)
            {
                carpeta = "fondoequipos";
            }

            return carpeta;
        }
        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = serverUrl + "/" + carpeta + "/" + fileName;
            return urlPath;
        }
    }
}



