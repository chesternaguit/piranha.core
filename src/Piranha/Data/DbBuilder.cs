﻿/*
 * Copyright (c) 2017 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using System.Data;

namespace Piranha
{
    public sealed class DbBuilder
    {
        /// <summary>
        /// Gets/sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets/sets the db connection.
        /// </summary>
        public IDbConnection Connection { get; set; }

        /// <summary>
        /// Gets/sets if the database should be updated
        /// to latest migration automatically.
        /// </summary>
        public bool Migrate { get; set; }
    }
}