// <copyright file="SnakeCasePropertyNamingPolicy.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Api
{
    using System.Linq;
    using System.Text.Json;

    /// <summary>
    /// Class for converting strings to snake case for JSON serialization.
    /// </summary>
    public class SnakeCasePropertyNamingPolicy : JsonNamingPolicy
    {
        /// <inheritdoc/>
        public override string ConvertName(string name)
        {
            return string.Concat(name.Select((character, index) =>
                    index > 0 && char.IsUpper(character)
                        ? "_" + character
                        : character.ToString()))
                .ToLower();
        }
    }
}