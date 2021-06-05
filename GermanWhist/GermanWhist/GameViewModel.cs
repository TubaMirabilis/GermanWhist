using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace GermanWhist
{
    class GameViewModel : ViewModelBase
    {
        int humanScore;
        public int HumanScore
        {
            get => humanScore;
            set => SetProperty(ref humanScore, value);
        }
        int cpuScore;
        public int CpuScore
        {
            get => cpuScore;
            set => SetProperty(ref cpuScore, value);
        }
        Card topCard;
        public Card TopCard
        {
            get => topCard;
            set => SetProperty(ref topCard, value);
        }
        public Card.SuitType TrumpSuit { get; init; }
        bool deckIsEmpty;
        public bool DeckIsEmpty
        {
            get => deckIsEmpty;
            set => SetProperty(ref deckIsEmpty, value);
        }
        public ObservableRangeCollection<Card> HumanCards { get; }
        public ObservableRangeCollection<Card> CpuCards { get; }
        public AsyncCommand<Card> SelectedCommand { get; }
        public GameViewModel()
        {
            HumanCards = new ObservableRangeCollection<Card>();
            CpuCards = new ObservableRangeCollection<Card>();
            SelectedCommand = new AsyncCommand<Card>(Selected);
            Deck.CreateDeck();
            Deck.Shuffle(3);
            HumanCards.AddRange(Deck.cards.GetRange(0, 13));
            CpuCards.AddRange(Deck.cards.GetRange(13, 13));
            Deck.cards.RemoveRange(0, 26);
            HumanScore = 0;
            CpuScore = 0;
            DeckIsEmpty = false;
            TopCard = Deck.cards[0];
            TrumpSuit = TopCard.Suit;
        }
        Card selectedCard;
        public Card SelectedCard
        {
            get => selectedCard;
            set => SetProperty(ref selectedCard, value);
        }
        async Task Selected(Card card)
        {
            if (card == null)
            {
                return;
            }
            Card cpuLead = CpuCards.ToList().Find(x => x.IsCpuSubmission == (true, true));
            if (cpuLead == null)
            {
                (Card cpuSubmission, bool cpuWinsFollowingSuit, bool cpuWinsByTrumping) cpuResponseDetails = HandleCpuResponse(card);
                IStrategy strategy = (cpuResponseDetails.cpuWinsFollowingSuit, cpuResponseDetails.cpuWinsByTrumping) switch
                {
                    var x when (cpuResponseDetails.cpuWinsFollowingSuit || cpuResponseDetails.cpuWinsByTrumping) => new CpuWinsStrategy(),
                    var y when (!cpuResponseDetails.cpuWinsFollowingSuit && !cpuResponseDetails.cpuWinsByTrumping) => new HumanWinsStrategy(),
                };
                await HandleStrategyBehaviour(strategy, cpuResponseDetails.cpuSubmission, card);
            }
            else
            {
                var humanValidMoves = HumanCards.Where(a => a.Suit == cpuLead.Suit).ToList();
                var competititveMoves = humanValidMoves.Where(a => a.FaceValue > cpuLead.FaceValue).ToList();
                var trumpingCards = cpuLead.Suit != TrumpSuit && !humanValidMoves.Any()
                             ? HumanCards.Where(a => a.Suit == TrumpSuit).ToList() : new System.Collections.Generic.List<Card>();
                if (competititveMoves.Any() && !competititveMoves.Contains(card))
                {
                    return;
                }
                else if (humanValidMoves.Any() && !humanValidMoves.Contains(card))
                {
                    return;
                }
                else if (trumpingCards.Any() && !trumpingCards.Contains(card))
                {
                    return;
                }
                else
                {
                    IStrategy strategy = (competititveMoves.Any(), trumpingCards.Any()) switch
                    {
                        var x when (!competititveMoves.Any() && !trumpingCards.Any()) => new CpuWinsStrategy(),
                        var y when (competititveMoves.Any() || trumpingCards.Any()) => new HumanWinsStrategy(),
                    };
                    await HandleStrategyBehaviour(strategy, cpuLead, card);
                }
            }
        }
        (Card, bool, bool) HandleCpuResponse(Card card)
        {
            var suit = card.Suit;
            var rank = card.FaceValue;
            var cpuValidMoves = CpuCards.Where(a => a.Suit == suit).OrderBy(a => a.FaceValue);
            var competititveMoves = cpuValidMoves.Where(a => a.FaceValue > rank).ToList();
            var trumpingCards = suit != TrumpSuit && !cpuValidMoves.Any()
                             ? CpuCards.Where(a => a.Suit == TrumpSuit).OrderBy(a => a.FaceValue).ToList()
                             : new System.Collections.Generic.List<Card>();
            var targetCard = competititveMoves.Any() ? competititveMoves.FirstOrDefault()
                             : cpuValidMoves.Any() ? cpuValidMoves.FirstOrDefault()
                             : trumpingCards.Any() ? trumpingCards.FirstOrDefault()
                             : CpuCards.OrderBy(a => a.FaceValue).FirstOrDefault();
            var indexOfTarget = CpuCards.ToList().FindIndex(x => x.Suit == targetCard.Suit  && x.FaceValue == targetCard.FaceValue);
            return (CpuCards[indexOfTarget], competititveMoves.Any(), trumpingCards.Any());
        }
        async Task HandleStrategyBehaviour(IStrategy strategy, Card card1, Card card2)
        {
            await strategy.MarkSelections(card1, card2);
            CpuCards.Remove(card1);
            HumanCards.Remove(card2);
            strategy.UpdateScore(this);
            strategy.TidyUp(CpuCards, HumanCards, Deck.cards);
            await HandleTopCardReplacement(Deck.cards.Count);
        }
        async Task HandleTopCardReplacement(int num)
        {
            if (num == 0)
            {
                DeckIsEmpty = true;
                await Task.Delay(App.AnimationDuration);
                return;
            }
            TopCard = Deck.cards[0];
        }
    }
}