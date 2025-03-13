using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    internal static class ResponseParser
    {
        public static bool ParseReadyOK(string response) => response?.Contains("readyok") == true;
        public static string ParseBestMove(string response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (response.StartsWith("bestmove "))
            {
                string[] parts = response.Split(' ');

                if (parts.Length < 2)
                    throw new InvalidDataException("Invalid bestmove format");

                if (parts[1] == "(none)")
                    return null;

                return parts[1];
            }

            throw new InvalidDataException("Response does not contain a bestmove");
        }
    }
}
