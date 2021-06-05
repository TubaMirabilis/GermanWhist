using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmHelpers;

namespace GermanWhist
{
    interface IStrategy
    {
        Task MarkSelections(Card cpuCard, Card humanCard);
        void UpdateScore(GameViewModel viewModel);
        void TidyUp(ObservableRangeCollection<Card> cpuCards, ObservableRangeCollection<Card> humanCards, List<Card> mainDeck);
    }
}