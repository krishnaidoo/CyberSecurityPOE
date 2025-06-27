using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CyberSecurityPOE
{
    public partial class QuizWindow : Window
    {
        private int currentQuestionIndex = 0;
        private int score = 0;

        private List<Question> questions = new List<Question>
        {
            new Question("What should you do if you receive an email asking for your password?",
                         new[] {"Reply with your password", "Delete the email", "Report it as phishing", "Ignore it"}, 2),
            new Question("What makes a password strong?",
                         new[] {"Using your pet's name", "Using 123456", "Short and simple words", "Long, complex, and unique phrases"}, 3),
            new Question("Which of these is a sign of phishing?",
                         new[] {"Unusual sender email", "Well-formatted message", "Trusted domain", "None of the above"}, 0),
            new Question("How often should you update your passwords?",
                         new[] {"Every 10 years", "Never", "Regularly", "Only when hacked"}, 2),
            new Question("What is 2FA?",
                         new[] {"Two friends arguing", "Two-factor authentication", "Too fast access", "Two-folder attachment"}, 1),
            new Question("Which of these is NOT a good browsing habit?",
                         new[] {"Using HTTPS sites", "Clicking unknown pop-ups", "Clearing cache", "Using a VPN"}, 1),
            new Question("What is social engineering?",
                         new[] {"Building networks", "Social media marketing", "Tricking people into giving information", "Creating social events"}, 2),
            new Question("Why avoid public Wi-Fi for banking?",
                         new[] {"Slow speeds", "Security risks", "It’s impolite", "No reason at all"}, 1),
            new Question("A safe way to store passwords is?",
                         new[] {"Write them on paper", "Save in browser", "Use a password manager", "Tell a friend"}, 2),
            new Question("Which action improves online security?",
                         new[] {"Ignore updates", "Use weak passwords", "Enable 2FA", "Click random ads"}, 2)
        };

        public QuizWindow()
        {
            InitializeComponent();
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex >= questions.Count)
            {
                QuestionText.Text = $"Quiz complete! You scored {score}/{questions.Count}.";
                OptionA.Visibility = OptionB.Visibility = OptionC.Visibility = OptionD.Visibility = Visibility.Collapsed;
                NextButton.Visibility = Visibility.Collapsed;
                FeedbackText.Text = "";

                // Optionally log result to activity log
                if (Application.Current.MainWindow is MainWindow mainWin)
                {
                    mainWin.LogActivity($"Quiz completed: {score}/{questions.Count} correct.");
                }

                return;
            }

            var q = questions[currentQuestionIndex];
            QuestionText.Text = $"Q{currentQuestionIndex + 1}. {q.Text}";
            OptionA.Content = $"A) {q.Options[0]}";
            OptionB.Content = $"B) {q.Options[1]}";
            OptionC.Content = $"C) {q.Options[2]}";
            OptionD.Content = $"D) {q.Options[3]}";

            FeedbackText.Text = "";
            NextButton.Visibility = Visibility.Collapsed;
            SetButtonsEnabled(true);
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = (Button)sender;
            int selectedIndex = selectedButton == OptionA ? 0 :
                                selectedButton == OptionB ? 1 :
                                selectedButton == OptionC ? 2 : 3;

            var q = questions[currentQuestionIndex];

            if (selectedIndex == q.CorrectIndex)
            {
                FeedbackText.Text = "✅ Correct!";
                FeedbackText.Foreground = System.Windows.Media.Brushes.Green;
                score++;
            }
            else
            {
                FeedbackText.Text = $"❌ Incorrect. Correct answer: {(char)('A' + q.CorrectIndex)}";
                FeedbackText.Foreground = System.Windows.Media.Brushes.Red;
            }

            SetButtonsEnabled(false);
            NextButton.Visibility = Visibility.Visible;
        }

        private void SetButtonsEnabled(bool isEnabled)
        {
            OptionA.IsEnabled = OptionB.IsEnabled = OptionC.IsEnabled = OptionD.IsEnabled = isEnabled;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            DisplayQuestion();
        }
    }

    public class Question
    {
        public string Text { get; }
        public string[] Options { get; }
        public int CorrectIndex { get; }

        public Question(string text, string[] options, int correctIndex)
        {
            Text = text;
            Options = options;
            CorrectIndex = correctIndex;
        }
    }
}
