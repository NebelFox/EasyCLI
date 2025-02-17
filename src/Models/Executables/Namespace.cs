﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Verbox.Models.Executables
{
    internal sealed class Namespace : Executable
    {
        private readonly IReadOnlyDictionary<string, Executable> _executables;

        public Namespace(string help,
                         IEnumerable<(string, Executable)> executables) : base(help)
        {
            _executables = new Dictionary<string, Executable>(
                executables.Select(
                    pair => new KeyValuePair<string, Executable>(pair.Item1, pair.Item2)));
        }

        public override void Execute(Box box, string[] tokens)
        {
            if (tokens.Contains(HelpSwitch) && tokens.Length == 1)
            {
                Help();
            }
            else
            {
                if (tokens.Length == 0)
                    throw new ArgumentException("No command name provided");
                
                Execute(box, tokens[0], tokens[1..]);
            }
        }

        private void Execute(Box box, string name, string[] tokens)
        {
            if (!_executables.TryGetValue(name, out Executable executable))
                throw new ArgumentException($"Unknown command: '{name}'");
            executable.Execute(box, tokens);
        }
    }
}
