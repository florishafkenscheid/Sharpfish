﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpfish
{
    public static class ResponseParser
    {
        public static bool ParseReadyOK(string response) => response?.Contains("readyok") == true;
        public static string ParseBestMove(string response)
        {
            ArgumentNullException.ThrowIfNull(response);

            if (response.StartsWith("bestmove "))
            {
                string[] parts = response.Split(' ');

                if (parts.Length < 2)
                    throw new InvalidDataException("Invalid bestmove format");

                if (parts[1] == "(none)")
                {
                    throw new Exception("No valid best move was found");
                }

                return parts[1];
            }

            throw new InvalidDataException("Response does not contain a bestmove");
        }

        public static string ParseEvaluation(string response)
        {
            try
            {
                return response.Split([' '], StringSplitOptions.RemoveEmptyEntries)[2];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidDataException("Invalid evaluation format: not enough parts in the response");
            }
        }

        public static string[] ParsePV(string response)
        {
            ArgumentNullException.ThrowIfNull(response);

            if (response.StartsWith("info"))
            {
                return response
                    .Split(' ')
                    .Skip(21) // Skip the first 21 elements (info part)
                    .ToArray();
            }
            else
            {
                return [];
            }
        }
    }
}
