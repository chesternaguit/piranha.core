﻿/*
 * Copyright (c) 2016 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

namespace Piranha.EF.Data
{
    public interface ISlug
    {
        /// <summary>
        /// Gets/sets the display title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets/sets the unique slug.
        /// </summary>
        string Slug { get; set; }
    }
}
