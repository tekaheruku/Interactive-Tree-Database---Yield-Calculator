using System;
using System.Text;

namespace TreeYield.Core.Data
{
    /// <summary>Small helpers for building safe SQL text.</summary>
    public static class SqlText
    {
        /// <summary>Escape character used in every LIKE ... ESCAPE clause of this application.</summary>
        public const char LikeEscapeCharacter = '\\';

        /// <summary>
        /// Escapes the LIKE wildcards %, _ and [ (and the escape character itself) so that
        /// user input is matched literally. The value is still sent as a parameter; this
        /// only stops "50%" from meaning "50 followed by anything".
        /// </summary>
        public static string EscapeLikePattern(string text)
        {
            if (text == null)
            {
                return null;
            }

            var escaped = new StringBuilder(text.Length + 8);
            foreach (char c in text)
            {
                if (c == LikeEscapeCharacter || c == '%' || c == '_' || c == '[')
                {
                    escaped.Append(LikeEscapeCharacter);
                }

                escaped.Append(c);
            }

            return escaped.ToString();
        }

        /// <summary>
        /// Wraps a database name in square brackets, doubling any closing bracket.
        /// Needed for CREATE DATABASE, which cannot take a parameter.
        /// </summary>
        public static string QuoteIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 128)
            {
                throw new ArgumentException("A database name must be 1 to 128 characters long.", nameof(name));
            }

            return "[" + name.Replace("]", "]]") + "]";
        }
    }
}
