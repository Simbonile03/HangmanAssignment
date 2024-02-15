using System.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;


namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage, INotifyPropertyChanged
    {
        private List<string> wordList = new List<string>
        {
            "fourtyfour",
            "work",
            "faculty",
            "elevator",
            "simbonile",
            "oxygen"
            // Add more words as needed
        };

        private string _chosenWord = "";
        private string _guessedWord = "";
        private int _maxAttempts = 9; // Maximum attempts allowed (including additional attempts)
        private int _remainingAttempts;
        private string _guessedLetters = "";
        private string _guessInput = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string ChosenWord
        {
            get { return _chosenWord; }
            set
            {
                _chosenWord = value;
                OnPropertyChanged(nameof(ChosenWord));
            }
        }

        public string GuessedWord
        {
            get { return _guessedWord; }
            set
            {
                _guessedWord = value;
                OnPropertyChanged(nameof(GuessedWord));
            }
        }

        public int RemainingAttempts
        {
            get { return _remainingAttempts; }
            set
            {
                _remainingAttempts = value;
                OnPropertyChanged(nameof(RemainingAttempts));
            }
        }

        public string GuessedLetters
        {
            get { return _guessedLetters; }
            set
            {
                _guessedLetters = value;
                OnPropertyChanged(nameof(GuessedLetters));
            }
        }

        public string GuessInput
        {
            get { return _guessInput; }
            set
            {
                _guessInput = value;
                OnPropertyChanged(nameof(GuessInput));
            }
        }

        public HangmanGamePage()
        {
            InitializeComponent();
            StartNewGame();
            this.BindingContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartNewGame()
        {
            ChosenWord = SelectRandomWord();
            GuessedWord = HideLetters(ChosenWord);
            RemainingAttempts = _maxAttempts;
            GuessedLetters = "";
            HangmanImage.Source = "hang1.png"; // Set initial hangman image
        }

        private string SelectRandomWord()
        {
            return wordList[new Random().Next(wordList.Count)];
        }

        private string HideLetters(string word)
        {
            return new string('_', word.Length);
        }

        private void OnGuessClicked(object sender, EventArgs e)
        {
            if (GuessInput.Length == 0)
            {
                // Handle empty guess entry
                return;
            }

            char guess = GuessInput.ToLower()[0];

            if (GuessedLetters.Contains(guess))
            {
                // Letter already guessed
                return;
            }

            GuessedLetters += guess;

            if (ChosenWord.Contains(guess))
            {
                // Correct guess
                char[] guessedWordArray = GuessedWord.ToCharArray();
                for (int i = 0; i < ChosenWord.Length; i++)
                {
                    if (ChosenWord[i] == guess)
                    {
                        guessedWordArray[i] = guess;
                    }
                }
                GuessedWord = new string(guessedWordArray);
            }
            else
            {
                // Incorrect guess
                RemainingAttempts--;

                int hangmanImageNumber = _maxAttempts - RemainingAttempts;
                HangmanImage.Source = $"hang{hangmanImageNumber}.png"; // Update hangman image
            }

            if (GuessedWord == ChosenWord)
            {
                // Player wins
                DisplayAlert("Congratulations!", $"You guessed the word: {ChosenWord}", "OK");
                StartNewGame(); // Start a new game
            }
            else if (RemainingAttempts == 0)
            {
                // Player loses
                DisplayAlert("Game Over", $"The word was: {ChosenWord}", "OK");
                StartNewGame(); // Start a new game
            }

            GuessInput = ""; // Clear the input field
        }
    }
}