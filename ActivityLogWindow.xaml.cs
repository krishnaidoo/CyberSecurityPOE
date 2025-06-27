using System;
using System.Collections.Generic;
using System.Windows;

namespace CyberSecurityPOE
{
    public partial class ActivityLogWindow : Window
    {
        public ActivityLogWindow(List<string> logEntries)
        {
            InitializeComponent();

            // Show only the last 10 entries
            int start = Math.Max(0, logEntries.Count - 10);
            foreach (string entry in logEntries.GetRange(start, logEntries.Count - start))
            {
                LogListBox.Items.Add(entry);
            }
        }
    }
}
