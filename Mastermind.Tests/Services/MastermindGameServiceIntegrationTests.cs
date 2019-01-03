﻿using Mastermind.Models;
using Mastermind.Services;
using Mastermind.Services.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mastermind.Tests.Services
{
    public class MastermindGameServiceIntegrationTests
    {
        readonly string correctAnswer = "ABCD";
        ICheckAnswersService checkAnswersService;
        IGameSettings gameSettings;
        IMastermindGame serviceUnderTest;
        List<string> answers;


        [SetUp]
        public void Setup()
        {
            checkAnswersService = new AnswerCheckService();
            gameSettings = new GameSettings(6, correctAnswer.Length);
            serviceUnderTest = new MastermindGameService(correctAnswer, checkAnswersService, gameSettings);
            answers = new List<string> { "AACC", "AADD", "ABDD", "ABDC", correctAnswer };
        }


        [Test]
        public void PlayIntegrationTest_ShouldBeFinished_WhenCorrectAnswerIsGuessed()
        {
            // Arrange
            
            // Act
            foreach (var answer in answers)
            {
                if (!serviceUnderTest.LastCheck.IsCorrect)
                {
                    serviceUnderTest.PlayRound(answer);
                }
            }

            // Assert
            Assert.True(serviceUnderTest.LastCheck.IsCorrect);
            Assert.AreEqual(5, serviceUnderTest.Rounds);
        }
        
        [Test]
        public void PlayIntegrationTest_ShouldFinishAfter5Rounds_WhenCorrectAnswerIsGuessedIn5Round()
        {
            // Arrange
            int i = 0;

            // Act
            while (!serviceUnderTest.LastCheck.IsCorrect)
            {
                serviceUnderTest.PlayRound(answers[i]);
                ++i;
            }

            // Assert
            Assert.True(serviceUnderTest.LastCheck.IsCorrect);
            Assert.AreEqual(5, serviceUnderTest.Rounds);
        }

        [Test]
        public void PlayIntegrationTest_ShouldFinishAfterFirstRound_WhenCorrectAnswerIsGuessedInFirstRound()
        {
            // Arrange
            int i = 0;

            // Act
            while (!serviceUnderTest.LastCheck.IsCorrect)
            {
                serviceUnderTest.PlayRound(correctAnswer);
                ++i;
            }

            // Assert
            Assert.True(serviceUnderTest.LastCheck.IsCorrect);
            Assert.AreEqual(1, serviceUnderTest.Rounds);
        }
    }
}
