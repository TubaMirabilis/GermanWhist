using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;

namespace GermanWhist
{
    class CpuWinsStrategy : IStrategy
    {
        public async Task MarkSelections(Card cpuCard, Card humanCard)
        {
            cpuCard.IsCpuSubmission = (true, true);
            humanCard.IsHumanSubmission = true;
            await Task.Delay(App.AnimationDuration);
        }
        public void UpdateScore(GameViewModel viewModel)
        {
            viewModel.CpuScore += 1;
        }
        public void TidyUp(ObservableRangeCollection<Card> cpuCards, ObservableRangeCollection<Card> humanCards, List<Card> mainDeck)
        {
            if(!mainDeck.Any())
            {
                LeadTheNextTrick(cpuCards);
                return;
            }
            cpuCards.Add(mainDeck[0]);
            mainDeck.Remove(mainDeck[0]);
            humanCards.Add(mainDeck[0]);
            mainDeck.Remove(mainDeck[0]);
            LeadTheNextTrick(cpuCards);
        }
        void LeadTheNextTrick(ObservableRangeCollection<Card> cpuCards)
        {
            if(cpuCards.Any())
            {
                var rnd = new Random().Next(0, cpuCards.Count - 1);
                cpuCards[rnd].IsCpuSubmission = (true, true);
            }
        }
    }
}