﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.ApiHub.Management.Entities
{
    public class ApiId : ArmResourceId
    {
        public override string ToString()
        {
            return $"{Id}/{Name}";
        }
    }
}
