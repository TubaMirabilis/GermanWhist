using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;

namespace GermanWhist
{
    class HumanWinsStrategy : IStrategy
    {
        public async Task MarkSelections(Card cpuCard, Card humanCard)
        {
            humanCard.IsHumanSubmission = true;
            await Task.Delay(App.AnimationDuration);
            cpuCard.IsCpuSubmission = (true, false);
            await Task.Delay(App.AnimationDuration);
        }
        public void UpdateScore(GameViewModel viewModel)
        {
            viewModel.HumanScore += 1;
        }
        public void TidyUp(ObservableRangeCollection<Card> cpuCards, ObservableRangeCollection<Card> humanCards, List<Card> mainDeck)
        {
            if (!mainDeck.Any())
            {
                return;
            }
            humanCards.Add(mainDeck[0]);
            Deck.cards.Remove(mainDeck[0]);
            cpuCards.Add(mainDeck[0]);
            Deck.cards.Remove(mainDeck[0]);
        }
    }
}