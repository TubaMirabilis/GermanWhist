using System;
using Xamarin.Forms;

namespace GermanWhist
{
    class Card : ViewModelBase
    {
        public Card(Rank value, SuitType suit)
        {
            if (!Enum.IsDefined(typeof(SuitType), suit))
                throw new ArgumentOutOfRangeException("suit");
            if (!Enum.IsDefined(typeof(Rank), value))
                throw new ArgumentOutOfRangeException("rank");

            Suit = suit;
            FaceValue = value;
            ImageString = SetImageString(this);
            IsCpuSubmission = (false, false);
            IsHumanSubmission = false;
        }
        public enum SuitType
        {
            Clubs,
            Spades,
            Hearts,
            Diamonds
        }
        public enum Rank
        {
            Two = 2,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }
        public Rank FaceValue { get; set; }
        public SuitType Suit { get; set; }
        public string ImageString { get; set; }
        public ImageSource Image
        {
            get => ImageSource.FromResource(ImageString);
        }
        private (bool, bool) isCpuSubmission;
        public (bool isSelected, bool imageIsInanimate) IsCpuSubmission
        {
            get => isCpuSubmission;
            set => SetProperty(ref isCpuSubmission, value); 
        }
        private bool isHumanSubmission;
        public bool IsHumanSubmission
        {
            get => isHumanSubmission;
            set => SetProperty(ref isHumanSubmission, value);
        }
        string SetImageString(Card card)
        {
            var abbreviatedSuit = card.Suit.ToString();
            var abbreviatedRank = (int)card.FaceValue;
            return $"GermanWhist.img.{abbreviatedSuit}{abbreviatedRank}.jpg";
        }
    }
}