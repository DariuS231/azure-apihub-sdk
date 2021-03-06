﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.ApiHub.Table.Protocol
{
    /// <summary>
    /// Represents the metadata for a table.
    /// </summary>
    [DataContract]
    internal class TableMetadata
    {
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets table title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets table permission.
        /// </summary>
        [DataMember(Name = "x-ms-permission")]
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the capabilities.
        /// </summary>
        [DataMember(Name = "x-ms-capabilities", EmitDefaultValue = false)]
        public TableCapabilitiesMetadata Capabilities { get; set; }

        /// <summary>
        /// Gets the table schema.
        /// </summary>
        [DataMember(Name = "schema")]
        public JObject Schema { get; set; }
    }
}
