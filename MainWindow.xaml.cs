using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CyberSecurityPOE
{
    public partial class MainWindow : Window
    {
        public void LogActivity(string message)
        {
            activityLog.Add($"{DateTime.Now:T} - {message}");
        }

        private Dictionary<string, string> keywordResponses;
        private List<string> phishingTips;
        private List<string> activityLog = new List<string>();
        private List<string> userTasks = new List<string>(); // ✅ Added this
        private string? rememberedTopic = null;

        public MainWindow()
        {
            InitializeComponent();
            LoadChatbotData();
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            QuizWindow quizWindow = new QuizWindow();
            quizWindow.Show();
        }

        private void LoadChatbotData()
        {
            keywordResponses = new Dictionary<string, string>
            {
                { "password", "Use strong, unique passwords for each account." },
                { "phishing", "Never click on suspicious links or give out personal information." },
                { "scam", "Verify sources before responding to messages that seem unusual." },
                { "privacy", "Check your app and social media privacy settings regularly." }
            };

            phishingTips = new List<string>
            {
                "Don't trust emails asking for login info.",
                "Hover over links before clicking to see where they go.",
                "Look for grammar mistakes in scam emails."
            };
        }

        private void AskButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInputBox.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                ChatHistory.Items.Add("Bot: Please enter a valid question.");
                return;
            }

            ChatHistory.Items.Add("You: " + input);
            activityLog.Add($"You asked: {input}");

            //  NLP /Reminder Detection 
            if (input.Contains("remind me") || input.Contains("set a reminder") ||
                input.Contains("add a task") || input.Contains("reminder") ||
                input.Contains("task") || input.Contains("to-do") || input.Contains("remember to"))
            {
                string taskText = input.Replace("remind me to", "")
                                       .Replace("set a reminder to", "")
                                       .Replace("add a task to", "")
                                       .Replace("remember to", "")
                                       .Trim();

                if (string.IsNullOrWhiteSpace(taskText))
                    taskText = "Unnamed Task";

                userTasks.Add(taskText);
                activityLog.Add($"{DateTime.Now:T} - NLP Task Added: {taskText}");

                ChatHistory.Items.Add($"Bot: Got it! I’ve added a task to: '{taskText}'");
                return;
            }

            // ✅ Keyword match
            foreach (var keyword in keywordResponses)
            {
                if (input.Contains(keyword.Key))
                {
                    rememberedTopic = keyword.Key;
                    string response = keywordResponses[keyword.Key];
                    ChatHistory.Items.Add("Bot: " + response);
                    activityLog.Add($"Bot responded about: {keyword.Key}");
                    return;
                }
            }

            // ✅ Random phishing tip
            if (input.Contains("phishing"))
            {
                string randomTip = phishingTips[new Random().Next(phishingTips.Count)];
                ChatHistory.Items.Add("Bot: " + randomTip);
                activityLog.Add("Bot gave phishing tip.");
                return;
            }

            // ❌ No match
            ChatHistory.Items.Add("Bot: Sorry, I don't understand. Try asking about phishing, passwords, scams, or privacy.");
            activityLog.Add("Bot could not answer.");
        }

        private void ShowActivityLog_Click(object sender, RoutedEventArgs e)
        {
            ChatHistory.Items.Add("Bot: Here's a summary of recent actions:");
            int count = 1;
            foreach (string log in activityLog.GetRange(Math.Max(0, activityLog.Count - 10), Math.Min(10, activityLog.Count)))
            {
                ChatHistory.Items.Add($"{count++}. {log}");
            }
        }
    }
}
