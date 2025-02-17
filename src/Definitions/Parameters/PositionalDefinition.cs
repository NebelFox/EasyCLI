﻿namespace Verbox.Definitions.Parameters
{
    internal record PositionalDefinition(string Name,
                                         string Type,
                                         ArgTags Tags)
    {
        public override string ToString()
        {
            return $"{Prefix}{Name}{RepresentType()}{RepresentCollective()}{Postfix}";
        }

        private string Prefix => Tags.HasFlag(ArgTags.Optional) ? "[" : string.Empty;
        private string RepresentType() => Type != "string" ? $":{Type}" : string.Empty;
        private string RepresentCollective() => Tags.HasFlag(ArgTags.Collective) ? "..." : string.Empty;
        private string Postfix => Tags.HasFlag(ArgTags.Optional) ? "]" : string.Empty;
    }
}
