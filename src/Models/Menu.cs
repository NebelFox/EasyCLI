﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verbox.Models.Styles;
using Verbox.Parsers;

// ReSharper disable once CheckNamespace
namespace Verbox
{
    public sealed class Menu
    {
        private readonly Models.Executables.Namespace _commands;
        private readonly Style _style;
        private readonly Splitter _splitter;
        private bool _isRunning;

        internal Menu(string help,
                      Namespace rootNamespace,
                      Style style)
        {
            _style = style;
            _commands = rootNamespace.Build(style, help);
            _splitter = new Splitter(_style.Input.Separator, _style.Input.Quotes);
        }

        public void StartDialogue()
        {
            _isRunning = true;
            Console.WriteLine(_style.Dialogue.Greeting);
            Console.Write(_style.Dialogue.SemanticSeparator);
            while (_isRunning)
                Prompt();

            Console.WriteLine(_style.Dialogue.Farewell);
        }

        public void Prompt()
        {
            Console.Write(_style.Dialogue.PromptIndicator);

            var inputs = new LinkedList<string>();
            do
            {
                inputs.AddLast(Console.ReadLine());
            }
            // ReSharper disable once PossibleNullReferenceException
            while (inputs.Last.Value.EndsWith(_style.Input.NewLineEscape));

            var input = string.Join(_style.Input.Separator,
                                    inputs.Select(i => i.TrimEnd(_style.Input.NewLineEscape)));

            try
            {
                Execute(input);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.Write(_style.Dialogue.SemanticSeparator);
            }
        }

        public void Execute(string command)
        {
            string[] tokens = _splitter.Split(command).ToArray();
            _commands.Execute(this, tokens);
        }

        public void Terminate()
        {
            _isRunning = false;
        }

        public void Help()
        {
            _commands.Help();
        }
    }
}