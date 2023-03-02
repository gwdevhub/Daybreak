using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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

    public event EventHandler<Map?>? MapClicked;
    public event EventHandler<Quest?>? QuestClicked;

    public QuestLogTemplate()
    {
        this.quests = new List<QuestMetadata>();
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

        this.ItemStackPanel.Children.Clear();

        foreach(var grouping in this.questLogCache)
        {
            var location = grouping.Key;
            var locationTextBlock = new OpaqueButton
            {
                Text = grouping.Key?.Name ?? UncategorizedQuestsString,
                FontSize = 18,
                Cursor = grouping.Key is null ? Cursors.Arrow : Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                Highlight = Brushes.White,
                HighlightOpacity = grouping.Key is null ? 0 : 0.6
            };

            locationTextBlock.MouseLeftButtonDown += (_, _) => this.OnMapClicked(location);
            this.ItemStackPanel.Children.Add(locationTextBlock);
            var rectangle = new Rectangle
            {
                Height = 1,
            };

            var rectangleForegroundBinding = new Binding("Foreground")
            {
                Source = this
            };

            rectangle.SetBinding(Rectangle.FillProperty, rectangleForegroundBinding);
            this.ItemStackPanel.Children.Add(rectangle);
            foreach (var questMetadata in grouping)
            {
                var quest = questMetadata.Quest;
                var questTextBlock = new OpaqueButton
                {
                    Text = quest?.Name,
                    FontSize = 20,
                    Cursor = Cursors.Hand,
                    Highlight = Brushes.White,
                    HighlightOpacity = 0.6,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextHorizontalAlignment = HorizontalAlignment.Left
                };

                questTextBlock.MouseLeftButtonDown += (_, _) => this.OnQuestClicked(quest);
                this.ItemStackPanel.Children.Add(questTextBlock);
            }

            this.ItemStackPanel.Children.Add(new Rectangle
            {
                Fill = Brushes.Transparent,
                Height = 10,
                HorizontalAlignment = HorizontalAlignment.Stretch
            });
        }
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

    private void OnMapClicked(Map? map)
    {
        this.MapClicked?.Invoke(this, map);
    }

    private void OnQuestClicked(Quest? quest)
    {
        this.QuestClicked?.Invoke(this, quest);
    }
}
