﻿using Mastermind.Models;
using Mastermind.Services;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace Mastermind.Tests.Services
{
    public class MastermindGameServiceTests
    {
        const string correctAnswer = "ABBAC";
        ICheckAnswersService checkAnswersService;
        MastermindGameService _serviceUnderTest;

        readonly string answerToCheck = "fooBar";
        IAnswerCheckDto answerCheck = Substitute.For<IAnswerCheckDto>();

        [SetUp]
        public void Setup()
        {
            checkAnswersService = Substitute.For<ICheckAnswersService>();
            _serviceUnderTest = new MastermindGameService(correctAnswer, checkAnswersService);
        }

        [Test]
        public void AnswerLength_ShouldBeCorrectAnswerLength_OnStart()
        {
            Assert.AreEqual(correctAnswer.Length, _serviceUnderTest.AnswerLength);
        }

        [Test]
        public void Rounds_ShouldBe0_OnStart()
        {
            Assert.AreEqual(0, _serviceUnderTest.Rounds);
        }


        [Test]
        public void Rounds_ShouldBe1_AfterOneRound()
        {
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(1, _serviceUnderTest.Rounds);
        }


        [Test]
        public void Rounds_ShouldBe2_After2Rounds()
        {
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            _serviceUnderTest.PlayRound(answerToCheck);
            _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(2, _serviceUnderTest.Rounds);
        }

        [Test]
        public void IsFinished_ShouldBeFalse_OnStartup()
        {
            Assert.AreEqual(false, _serviceUnderTest.IsFinished);
        }

        [Test]
        public void IsFinished_ShouldBeFalse_AfterRoundWithWrongAnswer()
        {
            answerCheck.WhitePoints.Returns(correctAnswer.Length - 1);
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            var result = _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(false, _serviceUnderTest.IsFinished);
        }

        [Test]
        public void IsFinished_ShouldBeTrue_AfterRoundWithRightAnswer()
        {
            answerCheck.WhitePoints.Returns(correctAnswer.Length);
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            var result = _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(true, _serviceUnderTest.IsFinished);
        }

        [Test]
        public void PlayRound_ShouldReturnStatsFromService_GivenAnswerToCheck()
        {
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            var result = _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(answerCheck, result);
        }

        [Test]
        public void PlayRound_ShouldCacheAnswer_GivenAnswerToCheck()
        {
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            var result = _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(answerToCheck, _serviceUnderTest.Answers.First());
        }

        [Test]
        public void PlayRound_ShouldCacheAnswerCheck_GivenAnswerToCheck()
        {
            // Arrange
            checkAnswersService.CheckAnswer(correctAnswer, answerToCheck).Returns(answerCheck);

            // Act
            _serviceUnderTest.PlayRound(answerToCheck);

            // Assert
            Assert.AreEqual(answerCheck, _serviceUnderTest.AnswerChecks[answerToCheck]);
        }

        [Test]
        public void Play()
        {
            var correctAnswer = "ABCD";
            var checkAnswersService = new AnswerCheckService();
            var serviceUnderTest = new MastermindGameService(correctAnswer, checkAnswersService);

            var answers = new[] { "AACC", "AADD", "ABDD", "ABDC", "ABCD" };

            //foreach(var answer in answers)
            //{
            //    if(!serviceUnderTest.IsFinished)
            //}
            //while(serviceUnderTest.AnswerChecks.Count() == 0 || 
            //    !serviceUnderTest.AnswerChecks[serviceUnderTest.Answers.Last()].IsFinished)
            //{

            //}
        }
    }
}
