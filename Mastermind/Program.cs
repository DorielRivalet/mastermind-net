﻿using Mastermind.Models;
using Mastermind.Services;
using Mastermind.Services.Solvers;
using System;

namespace Mastermind
{
    class Program
    {
        static void Main(string[] args)
        {
            var _gameFactory = new GameFactory();
            var generator = new GenerateKeyRangesService();
            var _serviceUnderTests = new KnuthSolverService(generator);
            var colors = 8;
            var digits = 4;
            var roundsLimit = 5;
            var settings = new GameSettings(colors, digits, roundsLimit);
            var keys = generator.GenerateCodes(settings);

            // Arrange
            var answer = "12345";
            var mastermindGame = _gameFactory.PrepareGame(answer, colors, roundsLimit);

            // Act
            var result = _serviceUnderTests.SolveGame(mastermindGame);
            
            Console.WriteLine($"Got {result.Answer} expected {answer}");

            // var answer = "ABCD";
            // var colors = 6;
            // var roundsLimit = 6;
            
            // var gameFactory = new GameFactory();
            // var gameplay = gameFactory.PrepareDefaultGameplay();
            // var game = gameFactory.PrepareGame(answer, colors, roundsLimit);
            // gameplay.SolveGame(game);

            // Console.ReadLine();
        }
    }
}
