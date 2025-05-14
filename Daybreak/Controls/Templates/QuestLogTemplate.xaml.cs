using Daybreak.Controls.Buttons;
using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for QuestLogTemplate.xaml
/// </summary>
public partial class QuestLogTemplate : UserControl
{
    private const string UncategorizedQuestsString = "Uncategorized Quests";
    private List<IGrouping<Map?, QuestMetadata>>? questLogCache;

    [GenerateDependencyProperty]
    private List<QuestMetadata> quests;

    [GenerateDependencyProperty]
    private IEnumerable<QuestLogEntry> logEntries = default!;

    public event EventHandler<Map?>? MapClicked;
    public event EventHandler<Quest?>? QuestClicked;

    public QuestLogTemplate()
    {
        this.quests = [];
        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        
        if (e.Property == QuestsProperty)
        {
            this.DrawQuestLogLayout();
        }
    }

    private void DrawQuestLogLayout()
    {
        var questLog = this.Quests?.GroupBy(q => q.From).OrderBy(g => g.Key?.Name).ToList();
        if (!this.DetectQuestLogChange(questLog))
        {
            return;
        }

        this.questLogCache = questLog;
        if (this.questLogCache is null)
        {
            return;
        }

        var logEntries = new List<QuestLogEntry>();
        foreach(var grouping in this.questLogCache)
        {
            var location = grouping.Key?.Name ?? UncategorizedQuestsString;
            logEntries.Add(new QuestLocationEntry { Title = location, Map = grouping.Key });
            foreach(var questMetadata in grouping)
            {
                var quest = questMetadata.Quest;
                logEntries.Add(new QuestEntry { Title = quest?.Name ?? string.Empty, Quest = quest });
            }

            logEntries.Add(new QuestLogSeparator());
        }

        this.LogEntries = logEntries;
    }

    private bool DetectQuestLogChange(List<IGrouping<Map?, QuestMetadata>>? newQuestLog)
    {
        if (this.questLogCache is null)
        {
            return true;
        }

        if (newQuestLog is null)
        {
            return true;
        }

        if (this.questLogCache.Count != newQuestLog.Count)
        {
            return true;
        }

        for (var i = 0; i < this.questLogCache.Count; i++)
        {
            var grouping1 = this.questLogCache[i];
            var grouping2 = newQuestLog[i];
            if (grouping1.Key?.Id != grouping2.Key?.Id)
            {
                return true;
            }

            var cachedQuests = grouping1.ToList();
            var newQuests = grouping2.ToList();
            if (cachedQuests.Count != newQuests.Count)
            {
                return true;
            }

            for (var j = 0; j < cachedQuests.Count; j++)
            {
                if (cachedQuests[j].Quest?.Id != newQuests[j].Quest?.Id)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void LocationButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not MenuButton menuButton ||
            menuButton.DataContext is not QuestLocationEntry questLocationEntry ||
            questLocationEntry.Map is not Map map)
        {
            return;
        }

        this.MapClicked?.Invoke(this, map);
    }

    private void QuestButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not MenuButton menuButton ||
            menuButton.DataContext is not QuestEntry questEntry ||
            questEntry.Quest is not Quest quest)
        {
            return;
        }

        this.QuestClicked?.Invoke(this, quest);
    }
}
