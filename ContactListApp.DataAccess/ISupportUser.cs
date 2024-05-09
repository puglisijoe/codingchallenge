﻿using System.Security.Claims;

namespace ContactListApp.DataAccess {
    /// <summary>
    ///     Class has a <see cref="ClaimsPrincipal" /> property.
    /// </summary>
    public interface ISupportUser {
        /// <summary>
        ///     The <see cref="ClaimsPrincipal" /> logged in.
        /// </summary>
        ClaimsPrincipal User { get; set; }
    }
}