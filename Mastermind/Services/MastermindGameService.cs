﻿using Mastermind.Models;
using System.Collections.Generic;

namespace Mastermind.Services
{
    public class MastermindGameService
    {
        private readonly string _correctAnswer;
        private readonly ICheckAnswersService _checkAnswersService;
        private readonly List<string> _answers;

        public IEnumerable<string> Answers { get => _answers; }
        public Dictionary<string, IAnswerCheckDto> AnswerChecks { get; private set; }
        public int AnswerLength { get => _correctAnswer.Length; }

        public MastermindGameService(string correctAnswer, ICheckAnswersService checkAnswersService)
        {
            _checkAnswersService = checkAnswersService;
            _correctAnswer = correctAnswer;
            _answers = new List<string>();
            AnswerChecks = new Dictionary<string, IAnswerCheckDto>();
        }

        public IAnswerCheckDto Round(string answerToCheck)
        {
            _answers.Add(answerToCheck);
            var result = _checkAnswersService.CheckAnswer(_correctAnswer, answerToCheck);
            AnswerChecks[answerToCheck] = result;

            return result;
        }
    }
}
