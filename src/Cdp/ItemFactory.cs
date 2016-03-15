﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microfoft.WindowsAzure.ApiHub
{
    public class ItemFactory
    {
        public static IFolderItem Parse(string connectionString)
        {
            Uri runtimeEndpoint;
            string accessTokenScheme;
            string accessToken;

            ParseConnectionString(connectionString, out runtimeEndpoint, out accessTokenScheme, out accessToken);

            var folderItem = New(runtimeEndpoint, accessTokenScheme, accessToken);

            return folderItem;
        }

        private static void ParseConnectionString(string connectionString, out Uri runtimeEndpoint, out string accessTokenScheme, out string accessToken)
        {
            //TODO: this needs more clean up.
            runtimeEndpoint = null;
            accessTokenScheme = null;
            accessToken = null;

            var parts = connectionString.Split(';');

            if (parts.Length != 3)
            {
                throw new FormatException("Invalid connectionstring: " + connectionString);
            }

            foreach (var part in parts)
            {
                var keyValue = part.Split('=');

                if (keyValue.Length != 2)
                {
                    throw new FormatException("Invalid connectionstring: " + connectionString);
                }

                var key = keyValue[0];
                var value = keyValue[1];

                switch (key.ToLowerInvariant())
                {
                    case "endpoint":
                        runtimeEndpoint = new Uri(value);
                        break;
                    case "scheme":
                        accessTokenScheme = value;
                        break;
                    case "accesstoken":
                        accessToken = value;
                        break;
                    default:
                        throw new FormatException("Invalid connectionstring: " + connectionString);
                }
            }
        }

        private static IFolderItem New(Uri runtimeEndpoint, string scheme, string accessToken)
        {
            return new FolderItem
            {
                _cdpHelper = new CdpHelper(runtimeEndpoint, scheme, accessToken)
            };
        }
    }
}