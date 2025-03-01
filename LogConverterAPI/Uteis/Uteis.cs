using LogConverterAPI.Controllers;
using LogConverterAPI.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace LogConverterAPI.Uteis
{
    public static class Uteis
    {
        public static void CriarDiretorioSeNaoExistir(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}