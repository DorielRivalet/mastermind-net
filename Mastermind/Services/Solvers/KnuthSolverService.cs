﻿using Mastermind.Models;
using Mastermind.Models.Interfaces;
using Mastermind.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind.Services.Solvers
{
    public class KnuthSolverService : ASolverService
    {
        public KnuthSolverService(IGenerateKeyRangesService keyRangesGenerator)
            : base(keyRangesGenerator, new AnswerCheckService())
        {
        
        }

        public override IGameResultDto SolveGame(IMastermindGame mastermindGame)
        {
            // Knuth five-guess algorithm from wiki
            var dto = BuildInitialState(mastermindGame);
            
            dto.Answer = GetInitialKeyGuess(dto.Settings.Digits);

            for (dto.Round = 0; !IsGameFinished(dto); ++dto.Round)
            {
                dto.PossibleKeys.Remove(dto.Answer);
                dto.KeysLeft.Remove(dto.Answer);

                dto.LastCheck = dto.MastermindGame.PlayRound(dto.Answer);
                dto.Answer = dto.Answer;

                if (!dto.LastCheck.IsCorrect)
                {
                    PruneKeysLeft(dto.KeysLeft, dto);
                    // keysLeft.RemoveAll(key => IsKeyToBeRemoved(key, dto.Answer, dto.LastCheck));
                    
                    var maxScores = GetMinMax(dto);
                    dto.Answer = GetNextGuess(dto, maxScores);
                }
            }

            return new GameResultDto(dto.MastermindGame.LastCheck.IsCorrect, dto.Answer, dto.MastermindGame.RoundsPlayed);
        }

        private string GetNextGuess(IKnuthRoundStateDto dto, IEnumerable<string> maxScores)
        {
            foreach(var maxScoredCode in maxScores) {
                if (dto.KeysLeft.Contains(maxScoredCode)) 
                {
                    return maxScoredCode;
                }
            }
            foreach (var maxScoredCode in maxScores) {
                if(dto.PossibleKeys.Contains(maxScoredCode)) {
                    return maxScoredCode;
                }
            }
            throw new InvalidOperationException("No minimax scores for possible keys!");
        }

        protected IKnuthRoundStateDto BuildInitialState(IMastermindGame mastermindGame)
        {
            var allKeys = _keyRangesGenerator.GenerateCodes(mastermindGame.Settings).ToList();

            var dto = new KnuthSolvingRoundStateDto()
            {
                Answer = string.Empty,
                Round = 0,
                MastermindGame = mastermindGame,
                PossibleKeys = allKeys.ToList(),
                KeysLeft = allKeys.ToList(),
            };

            return dto;
        }

// https://github.com/nattydredd/Mastermind-Five-Guess-Algorithm/blob/master/Five-Guess-Algorithm.cpp
        private IEnumerable<string> GetMinMax(IKnuthRoundStateDto dto)
        {
            var score = new Dictionary<string, int>();
            var scoreCount = new Dictionary<string, int>();

            foreach(var possibleKey in dto.PossibleKeys) {
                foreach(var keyLeft in dto.KeysLeft) {
                    var checkValue = CheckAnswer(possibleKey, keyLeft);
                    var check = $"{checkValue.WhitePoints}.{checkValue.BlackPoints}";
                    if(scoreCount.Keys.Contains(check)) 
                    {
                        var count = scoreCount[check];
                        scoreCount[check] = count + 1;
                    }
                    else
                    {
                        scoreCount[check] = 1;
                    }
                }
                var max = scoreCount.Values.Max();
                score[possibleKey] = max;
                scoreCount.Clear();
            }
            var min = score.Values.Min();
            var result = score.Keys
                .Where(k => score[k] == min)
                .OrderBy(k => k);

            return result;
        }
    }
}
